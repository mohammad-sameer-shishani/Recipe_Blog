using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recipe_Blog.Models;

namespace Recipe_Blog.Controllers
{
    public class ChefController : Controller
    {
        private readonly ModelContext _context;

        public ChefController(ModelContext context)
        {
            _context = context;
            
        }
        public void GetChefLoginInfo(){
            var _id = HttpContext.Session.GetInt32("chefSession");
            var _chef = _context.Users
                       .Include(u => u.Logins)
                       .SingleOrDefault(x => x.Id == _id);
            ViewBag.thisChef = _chef;
            ViewBag.thisChefLogin = _context.Logins.SingleOrDefault(x => x.UserId == _id);
        }
        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            GetChefLoginInfo();
            var modelContext = _context.Recipes.Include(r => r.Category).Include(r => r.User).OrderBy(x => x.Creationdate);
            return View(await modelContext.ToListAsync());
        }
        //GET: Chef 
        public async Task<IActionResult> MyRecipes()
        {
            GetChefLoginInfo();
            var myid = HttpContext.Session.GetInt32("chefSession");
            var modelContext = _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .Where(x => x.UserId == myid)
                .OrderBy(x => x.Creationdate);
            return View(await modelContext.ToListAsync());
        }

        // GET: Chef/Details/5
        public async Task<IActionResult> AllDetails(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            GetChefLoginInfo();
            return View(recipe);
        }
        // GET: Chef/MyRecipeDetails/5
        public async Task<IActionResult> MyDetails(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            GetChefLoginInfo();
            return View(recipe);
        }

        // GET: Chef/Create
        public IActionResult Create()
        {
            GetChefLoginInfo(); 
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Chef/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,Description,Creationdate,Name,UserId,CategoryId,RecipeStatusId")] Recipe recipe)
        {
            

            if (ModelState.IsValid)
            {
                var _id = Convert.ToDecimal(HttpContext.Session.GetInt32("chefSession"));
                recipe.UserId = _id;
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", recipe.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recipe.UserId);
            
            return View(recipe);
        }

        // GET: Chef/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {

            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            GetChefLoginInfo();
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", recipe.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recipe.UserId);
            return View(recipe);
        }

        // POST: Chef/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Name,Price,Description,CategoryId")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", recipe.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recipe.UserId);
            return View(recipe);
        }

        // GET: Chef/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Chef/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Recipes == null)
            {
                return Problem("Entity set 'ModelContext.Recipes'  is null.");
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: ChefCategories
        public async Task<IActionResult> Categories()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Categories'  is null.");
        }

        // GET: ChefCategories/Details/5
        public async Task<IActionResult> GategoryDetails(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        private bool RecipeExists(decimal id)
        {
          return (_context.Recipes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
