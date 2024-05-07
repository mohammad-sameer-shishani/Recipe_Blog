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
    public class ChefRecipesController : Controller
    {
        private readonly ModelContext _context;

        public ChefRecipesController(ModelContext context)
        {
            _context = context;
            
        }
        public void GetChefLoginInfo(){
            var id = HttpContext.Session.GetInt32("chefSession");
            var chef = _context.Users
                       .Include(u => u.Logins)
                       .SingleOrDefault(x => x.Id == id);
            ViewBag.thisChef = chef;
            ViewBag.thisChefLogin = _context.Logins.SingleOrDefault(x => x.UserId == id);
        }
        // GET: ChefRecipes
        public async Task<IActionResult> Index()
        {
            GetChefLoginInfo();
            var modelContext = _context.Recipes.Include(r => r.Category).Include(r => r.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: ChefRecipes/Details/5
        public async Task<IActionResult> Details(decimal? id)
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

        // GET: ChefRecipes/Create
        public IActionResult Create()
        {
            GetChefLoginInfo(); 
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ChefRecipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,Description,Creationdate,Name,UserId,CategoryId")] Recipe recipe)
        {
            var _id = Convert.ToDecimal(HttpContext.Session.GetInt32("chefSession"));

            if (ModelState.IsValid)
            {
                recipe.UserId = _id;
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", recipe.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recipe.UserId);
            
            return View(recipe);
        }

        // GET: ChefRecipes/Edit/5
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

        // POST: ChefRecipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Price,Description,Creationdate,Name,UserId,CategoryId")] Recipe recipe)
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

        // GET: ChefRecipes/Delete/5
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

        // POST: ChefRecipes/Delete/5
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

        private bool RecipeExists(decimal id)
        {
          return (_context.Recipes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
