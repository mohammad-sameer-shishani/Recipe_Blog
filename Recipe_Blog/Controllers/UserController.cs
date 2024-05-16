using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recipe_Blog.Models;

namespace Recipe_Blog.Controllers
{
    public class UserController : Controller
    {
        private readonly ModelContext _context;

        public UserController(ModelContext context)
        {
            _context = context;
        }
        // GET: UserController
        public async Task<ActionResult> Index()
        {
            var users = _context.Users.ToList(); 
            var categories = _context.Categories.ToList(); 
            var recipes=_context.Recipes.ToList();
            var testimonials= _context.Testimonials.ToList();
            var chef=_context.Users.Where(u=>u.RoleId==2).ToList();
            var home= _context.Homepages.ToList();
            var contact=_context.Contactus.ToList();
            var model = Tuple.Create<IEnumerable<User>, IEnumerable<Category>, IEnumerable<Recipe>, IEnumerable<Testimonial>, IEnumerable<User>, IEnumerable<Homepage>>(users,categories,recipes,testimonials,chef,home); 
            
            return View(model);
        }
        //GET : contactus 
        public  ActionResult ContactUs()
        {
                
            return View( );
        }
        [HttpPost]
        public ActionResult ContactUs( [Bind("Id,Username,Useremail,Subject,Message")]Contactu c) 
        {
            if (ModelState.IsValid)
            {
                _context.Add(c);
               _context.SaveChangesAsync();
                return RedirectToAction(nameof(ContactUs));
            }

            return View(c);
        }

        // GET: UserController
        public async Task<ActionResult> Testimonials()
        {
            var testimonials = _context.Testimonials
            .Include(u=>u.User).Where(u=>u.User.Id==u.UserId)
            .ToList();
            return View(testimonials);
        }

        // GET: UserCategories
        public async Task<IActionResult> Categories()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Categories'  is null.");
        }
        // GET: UserCategories/Details/5
        public async Task<IActionResult> CategoryDetails(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(u=>u.Recipes)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.Category = category;
            return View(category);
        }
        // GET: Recipes
        public async Task<IActionResult> Recipes()
        {
            var modelContext = _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .OrderBy(x => x.Creationdate);
            return View(await modelContext.ToListAsync());
        }
        // GET: RecipeDetails
        public async Task<IActionResult> RecipeDetails(decimal? id)
        {
            var modelContext = _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .SingleOrDefaultAsync(x => x.Id == id);
            return View(await modelContext);
        }

        //GET : Chefs
        public async Task<IActionResult> Chefs()
		{
			var modelContext = _context.Users
				.Include(r => r.Recipes)
				.Include(r => r.Logins)
                .Where(r=>r.RoleId==2)
				.OrderBy(x => x.Recipes.Count());
			return View(await modelContext.ToListAsync());
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
        public ActionResult Shop()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/CreateTestimonials
        public ActionResult CreateTestimonials()
        {
            return View();
        }

        // POST: UserController/CreateTestimonials
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTestimonials(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: UserController/AcceptedTestimonials
        public async Task<ActionResult> AcceptedTestimonials()
        {
            var model=_context.Testimonials.Include(u=>u.User).Where(u=>u.TestimonialStatusId==2).ToListAsync();
            return View(await model);
        }
        
        //GET: user/Checkout
        public async Task<IActionResult> Checkout(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .Include(r => r.User.Logins)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            var r = _context.Recipes.SingleOrDefault(u=>u.Id==id);
            ViewBag.currentUser = HttpContext.Session.GetInt32("userSession");

            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Purchase purchase)
        {
            if (!ModelState.IsValid)
            {
                return View(purchase);
            }

            var userData = await _context.Users.Include(v => v.Visas).SingleOrDefaultAsync();
            var visaData = _context.Visas.SingleOrDefaultAsync(check => check.Id == purchase.UserId);
            if (visaData == null) return NotFound();
            //ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", request.RecipeId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", request.UserId);
            foreach (var i in userData.Visas)
            {
                if (i.Amount < purchase.Amount)
                {
                    ModelState.AddModelError("", "you dont have a money");
                    return View(purchase);
                }
                if (i.Cardnumber != purchase.Cardnumber)
                {
                    ModelState.AddModelError("", "your card number is invalid");
                    return View(purchase);
                }
                if (i.Cvc != purchase.Cvc)
                {
                    ModelState.AddModelError("", "your Cvc number is invalid");
                    return View(purchase);
                }
                //if (i.Expdate != purchase.Expdate)
                //{
                //    ModelState.AddModelError("", "your Expire date number is invalid");
                //    return View(purchase);
                //}
                if (i.Nameoncard.ToLower() != purchase.Nameoncard.ToLower().Trim())
                {
                    ModelState.AddModelError("", "your Name is invalid");
                    return View(purchase);
                }
            }
            Request request = new();
            request.UserId = purchase.UserId;
            request.RecipeId = purchase.RecipeId;
            request.Requestdate = DateTime.Now;
            _context.Add(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool CategoryExists(decimal id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
