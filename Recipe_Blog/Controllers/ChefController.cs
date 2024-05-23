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
        public async Task<IActionResult> Index()
        {

            return View();
        }
        //// GET: Users/Edit/5
        //public async Task<IActionResult> EditProfile(decimal? id)
        //{
        //    if (id == null || _context.Users == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["RoleId"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.RoleId);
        //    return View(user);
        //}

        //// POST: Users/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditProfile(decimal id, [Bind("Id,Firstname,Lastname,Birthdate,RoleId,Imgpath")] User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RoleId"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.RoleId);
        //    return View(user);
        //}

        //// GET: Users/ProfileDetails/5
        //public async Task<IActionResult> ProfileDetails(decimal? id)
        //{
        //    if (id == null || _context.Users == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users
        //        .Include(u => u.Role)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        public async Task<IActionResult> Chefs() 
        {
            var chefs =_context.Users
                .Include(c=>c.Recipes)
                .Include(c => c.Logins)
                .Where(c=>c.RoleId==2)
                .ToList();
            return View(chefs);
        }

        //GET : ChefDetails
        public async Task<IActionResult> ChefDetails(decimal? id)
        {
            var modelContext = _context.Users
                .Include(r => r.Recipes)
                .Include(r => r.Logins)
                .Include(r => r.Role)
                .Where(r => r.Id == id).SingleOrDefaultAsync();
            return View(await modelContext);
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
        public async Task<IActionResult> AllRecipes()
        {
            GetChefLoginInfo();
            var modelContext = _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .OrderBy(x => x.Creationdate);
            return View(await modelContext.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> AllRecipes(DateTime? startDate, DateTime? endDate, string? name)
        {
            GetChefLoginInfo();
            var result = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .OrderBy(x => x.Creationdate)
                .ToListAsync();


            if (startDate != null)
                result = result.Where(x => x.Creationdate >= startDate).ToList();

            if (endDate != null)
                result = result.Where(x => x.Creationdate <= endDate).ToList();

            if (!String.IsNullOrEmpty(name))
                result = result.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();

            return View(result);
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
        [HttpPost]
        public async Task<IActionResult> MyRecipes(DateTime? startDate, DateTime? endDate, string? name)
        {
            GetChefLoginInfo();
            var result = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .OrderBy(x => x.Creationdate)
                .ToListAsync();


            if (startDate != null)
                result = result.Where(x => x.Creationdate >= startDate).ToList();

            if (endDate != null)
                result = result.Where(x => x.Creationdate <= endDate).ToList();

            if (!String.IsNullOrEmpty(name))
                result = result.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();

            return View(result);
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
        public async Task<IActionResult> Create([Bind("Id,Price,Description,Creationdate,Name,UserId,CategoryId,RecipeStatusId,Instructions,Ingredients")] Recipe recipe)
        {
            

            if (ModelState.IsValid)
            {
                var _id = Convert.ToDecimal(HttpContext.Session.GetInt32("chefSession"));
                recipe.Imgpath = "shishani_recipe-removebg-preview.png";
                recipe.UserId = _id;
                recipe.RecipeStatusId = 1;
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
                        View(await _context.Categories.Include(x=>x.Recipes).ToListAsync()) :
                        Problem("Entity set 'ModelContext.Categories'  is null.");
        }

        // GET: ChefCategories/Details/5
        public async Task<IActionResult> CategoryDetails(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var Categories = await _context.Categories
                .Include(x=>x.Recipes)
                .Where(x=>x.Id==id)
                .SingleOrDefaultAsync();
            if (Categories == null)
            {
                return NotFound();
            }

            return View(Categories);
        }



        // GET: Users/Edit/5
        public async Task<IActionResult> EditProfile(decimal? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(decimal id, [Bind("Id,Firstname,Lastname,Birthdate,RoleId,Imgpath")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.RoleId);
            return View(user);
        }


        // GET: Users/EditAccount/5
        public async Task<IActionResult> EditAccount(decimal? id)
        {
            if (id == null || _context.Logins == null)
            {
                return NotFound();
            }

            var user = await _context.Logins.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/EditAccount/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(decimal id, [Bind("Id,Email,UserName,Password,UserId")] Login login)
        {
            if (id != login.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(login);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(login.Id))
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
            return View(login);
        }

        // GET: Users/ProfileDetails/5
        public async Task<IActionResult> ProfileDetails(decimal? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        // GET: Users/AccountDetails/5
        public async Task<IActionResult> AccountDetails(decimal? id)
        {
            if (id == null || _context.Logins == null)
            {
                return NotFound();
            }

            var login = await _context.Logins
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (login == null)
            {
                return NotFound();
            }

            return View(login);
        }




        private bool RecipeExists(decimal id)
        {
          return (_context.Recipes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool UserExists(decimal id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
