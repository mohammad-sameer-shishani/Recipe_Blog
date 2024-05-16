using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recipe_Blog.Models;

namespace Recipe_Blog.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(ModelContext context , IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        //GET : contactus 
        public async Task<ActionResult> ContactUs()
        {
            var contact =_context.Contactus.ToListAsync();
            return View(await contact);
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("adminSession");//=>5

            ViewBag.currentAdmin = _context.Users.SingleOrDefault(u => u.Id == id);

            ViewBag.userCount = _context.Users.Where(x => x.RoleId == 3).Count();
            ViewBag.chefCount = _context.Users.Where(x => x.RoleId == 2).Count();
            ViewBag.testimonialAcceptedCount = _context.Testimonials.Where(x => x.TestimonialStatus.Statusname == "Accepted").Count();
            ViewBag.testimonialPendingCount = _context.Testimonials.Where(x => x.TestimonialStatus.Statusname == "Pending").Count();
            ViewBag.testimonialRejectedCount = _context.Testimonials.Where(x => x.TestimonialStatus.Statusname == "Rejected").Count();
            ViewBag.recipeAcceptedCount = _context.Recipes.Where(x => x.RecipeStatus.Statusname == "Accepted").Count();
            ViewBag.recipePendingdCount = _context.Recipes.Where(x => x.RecipeStatus.Statusname == "Pending").Count();
            ViewBag.recipeRejectedCount = _context.Recipes.Where(x => x.RecipeStatus.Statusname == "Rejected").Count();
            ViewBag.CategoriesCount = _context.Categories.Count();
            

            return View();
        }
        // GET: Recipes
        public async Task<IActionResult> Recipes()
        {
            return _context.Recipes != null ?
                        View(await _context.Recipes.Include(u => u.Category).Include(u => u.User).Include(u => u.Comments).ToListAsync()) :
                        Problem("Entity set 'ModelContext.Categories'  is null.");
        }
        // GET: EditRecipe
        public async Task<IActionResult> EditRecipes(decimal? id)
        {
            return _context.Recipes != null ?
                        View(await _context.Recipes
                        .Include(u => u.Category)
                        .Include(u => u.User)
                        .Include(u => u.Comments)
                        .SingleOrDefaultAsync(u => u.Id == id)) :
                        Problem("Entity set 'ModelContext.Categories'  is null.");
        }
        // GET: RecipeDetails
        public async Task<IActionResult> RecipeDetails(decimal? id)
        {
            var modelContext = _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .Include(r => r.Comments)
                .SingleOrDefaultAsync(x => x.Id == id);
            return View(await modelContext);
        }
        // GET: Recipe/Delete/5
        public async Task<IActionResult> RecipeDelete(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var Recipe = await _context.Recipes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Recipe == null)
            {
                return NotFound();
            }

            return View(Recipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost, ActionName("RecipeDeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecipeDeleteConfirmed(decimal id)
        {
            if (_context.Recipes == null)
            {
                return Problem("Entity set 'ModelContext.Recipes'  is null.");
            }
            var Recipe = await _context.Recipes.FindAsync(id);
            if (Recipe != null)
            {
                _context.Recipes.Remove(Recipe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //GET: CreateCategory
        public IActionResult CreateCategory() 
        {

            return View();
        }
        //Post: CreateCategory
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + category.ImageFile.FileName;
                    string imagePath = Path.Combine("img/", imageName);
                    string fullPath = Path.Combine(wwwrootPath + "/User/img/", imageName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        category.ImageFile.CopyToAsync(fileStream);
                    }
                    category.Imgpath = imagePath;
                }


                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Categories));
            }
            return View(category);

        }
        // GET: EditCategory
        public async Task<IActionResult> EditCategory(decimal? id)
        {
            return _context.Categories != null ?
                        View(await _context.Categories
                        .Include(u => u.Recipes)
                        .SingleOrDefaultAsync(u => u.Id == id)) :
                        Problem("Entity set 'ModelContext.Categories'  is null.");
        }
        // GET: CategoryDetails
        public async Task<IActionResult> CategoryDetails(decimal? id)
        {
            var modelContext = _context.Categories
                .Include(r => r.Recipes)
                .SingleOrDefaultAsync(x => x.Id == id);
            return View(await modelContext);
        }
        // GET: admin/DeleteCategory/5
        public async Task<IActionResult> DeleteCategory(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var Recipe = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Recipe == null)
            {
                return NotFound();
            }

            return View(Recipe);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("CategoryDeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryDeleteConfirmed(decimal id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ModelContext.Recipes'  is null.");
            }
            var Categories = await _context.Categories.FindAsync(id);
            if (Categories != null)
            {
                _context.Categories.Remove(Categories);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Categories
        public async Task<IActionResult> Categories()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.Include(u => u.Recipes).ToListAsync()) :
                        Problem("Entity set 'ModelContext.Categories'  is null.");
        }
        // GET: Requests
        public async Task<IActionResult> Requests()
        {
            return _context.Requests != null ?
                        View(await _context.Requests.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Requests'  is null.");
        }
        private bool CategoryExists(decimal id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult Home()
        {
            var users = _context.Users.ToList();
            var categories = _context.Categories.ToList();
            var recipes = _context.Recipes.ToList();
            var testimonials = _context.Testimonials.ToList();
            var chef = _context.Users.Where(u => u.RoleId == 2).ToList();
            var home = _context.Homepages.ToList();
            var contact = _context.Contactus.ToList();
            var model = Tuple.Create<IEnumerable<User>, IEnumerable<Category>, IEnumerable<Recipe>, IEnumerable<Testimonial>, IEnumerable<User>, IEnumerable<Homepage>>(users, categories, recipes, testimonials, chef, home);

            return View(model);
        }
        public IActionResult Testimonials()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
       
        public async Task<IActionResult> RegisteredUsers()
        {
            var modelContext = await _context.Logins
                .Include(u => u.User)
                .Include(u => u.User.Role)
                .Where(u => u.User.Role.Roleid == 3).OrderBy(u => u.Id).ToListAsync();
            return View(modelContext);
        }
        public async Task<IActionResult> RegisteredChefs()
        {

            List<Login> modelContext = await _context.Logins
                .Include(c => c.User)
                .Include(r => r.User.Recipes)
                .Include(r => r.User.Role)
                .Where(check => check.User.Role.Roleid == 2)
                .OrderBy(x => x.Id).ToListAsync();

            return View(modelContext);
        }
        public IActionResult Profile(decimal _id)
        {
            Login l = _context.Logins.Include(x => x.User).SingleOrDefault(u => u.User.Id == _id);
            return View(l);
        }
        [HttpPost]
        public IActionResult Profile(decimal _id, [Bind("Firstname,Lastname,Gender")] User user)
        {
            if (ModelState.IsValid)
            {
                user = _context.Users.SingleOrDefault(u => u.Id == _id);
                _context.Update(user);
                _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }
        //GET : ChefDetails
        public async Task<IActionResult> ChefDetails(decimal? id)
        {
            var modelContext = _context.Users
                .Include(r => r.Recipes)
                .Include(r => r.Logins)
                .Where(r => r.Id == id).SingleOrDefaultAsync();
            return View(await modelContext);
        }
        //GET : ChefDetails
        public async Task<IActionResult> UserDetails(decimal? id)
        {
            var modelContext = _context.Users
                .Include(r => r.Testimonials)
                .Include(r => r.Requests)
                .Include(r => r.Logins)
                .Where(r => r.Id == id).SingleOrDefaultAsync();
            return View(await modelContext);
        }
        // GET: UserController/PendingTestimonials
        public async Task<ActionResult> PendingTestimonials()
        {
            var model = _context.Testimonials
                .Where(u => u.TestimonialStatus.Statusname == "Pending")
                .ToListAsync();
            return View(await model);
        }// GET: UserController/AcceptedTestimonials
        public async Task<ActionResult> AcceptedTestimonials()
        {
            var model = _context.Testimonials.Include(u => u.TestimonialStatus)
                .Where(u => u.TestimonialStatus.Statusname == "Accepted")
                .ToListAsync();
            return View(await model);
        }
        // GET: UserController/RejectedTestimonials
        public async Task<ActionResult> RejectedTestimonials()
        {
            var model = _context.Testimonials.Include(u => u.TestimonialStatus)
                .Where(u => u.TestimonialStatus.Statusname == "Rejected")
                .ToListAsync();
            return View(await model);
        }
        [HttpPost]
        public async Task<ActionResult> AcceptTestimonial(decimal? id, [Bind("Id,TestimonialStatusId")]Testimonial t)
        {
            if (ModelState.IsValid)
            {
                _context.Update(t);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(PendingTestimonials));
        }
        [HttpPost]
        public async Task<ActionResult> RejectTestimonial()
        {
            var model = _context.Testimonials.Include(u => u.TestimonialStatus)
                .Where(u => u.TestimonialStatus.Statusname == "Rejected")
                .ToListAsync();
            return View(await model);
        }
        // GET: EditTesti
        public async Task<IActionResult> EditTestimonial(decimal? id)
        {
            return _context.Testimonials != null ?
                        View(await _context.Testimonials
                        .SingleOrDefaultAsync(u => u.Id == id)) :
                        Problem("Entity set 'ModelContext.Categories'  is null.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTestimonial(decimal id, [Bind("Id,TestimonialStatusId")] Testimonial t)
        {
            if (id != t.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    _context.Update(t);
                    await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["tata"] = new SelectList(_context.Statuses, "Statusid", "Statusname", t.TestimonialStatusId);
            return View(t);
        }
      
    }
}
