using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recipe_Blog.Models;

namespace Recipe_Blog.Controllers
{
    public class AuthController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
             _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        //GET: Index
        public IActionResult Index()
		{
			return View();
		}
		public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([Bind("UserName,Password")]LoginViewModel login)
        {
			
			HttpContext.Session.Clear();
            if (!ModelState.IsValid)
			{
				return View(login);
			}

			var user = await _context.Logins
				.Include(u => u.User)
				.SingleOrDefaultAsync(u => (u.Email == login.UserName|| u.UserName == login.UserName) && u.Password == login.Password);

			if (user == null)
			{
				ModelState.AddModelError("", "invalid email or password");
				return View(login);
			}

			var userId_ = user.UserId;
			var roleId = user.User?.RoleId;

			if (roleId == null)
			{
				ModelState.AddModelError("", "User information is missing.");
				return View(login);
			}

			switch (roleId)
			{
				case 1: // Admin
					HttpContext.Session.SetInt32("adminSession", (int)userId_);
					
                    return RedirectToAction("Index", "Admin");

				case 2: // chef
					HttpContext.Session.SetInt32("chefSession", (int)userId_);
					var chefid = HttpContext.Session.GetInt32("chefSession");
					var chef_=_context.Users.SingleOrDefaultAsync(u => u.Id == chefid);
					ViewBag.thisChef = chef_;
                    return RedirectToAction("Index","Chef");

				case 3: // user/customer
					HttpContext.Session.SetInt32("userSession", (int)userId_);
                    var userid = HttpContext.Session.GetInt32("userSession");
                    var user_ = _context.Users.SingleOrDefaultAsync(u => u.Id == userid);
                    ViewBag.thisUser = user_;
                    return RedirectToAction("Index", "User");

				default:
					ModelState.AddModelError("","Something Error");
					return View(login);
			}

          
        }
        public IActionResult RegisterAsUser()
        {
            return View();
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegisterAsUser(UserViewModel userViewModel)
        {
			if (ModelState.IsValid)
            {
                User user = new User();
                user.Firstname = userViewModel.Firstname;
                user.Lastname = userViewModel.Lastname;
                user.RoleId = 3;
                user.Birthdate = userViewModel.Birthdate;
				user.Imgpath = "~/User/img/man-default.png";

				await _context.AddAsync(user);
				await _context.SaveChangesAsync();
                userViewModel.UserId = user.Id;

                Login login = new Login();
                login.Email = userViewModel.Email;
                login.UserName = userViewModel.UserName;
                login.Password = userViewModel.Password;
                login.UserId = userViewModel.UserId;
				await _context.AddAsync(login);
				await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Auth");
            }
            return View();
            
        }
		//GET: Register as chef
		public IActionResult RegisterAsChef()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegisterAsChef(UserViewModel userViewModel)
		{

			if (ModelState.IsValid)
			{
				User user = new User();
				user.Firstname = userViewModel.Firstname;
				user.Lastname = userViewModel.Lastname;
				user.RoleId = 2;
				user.Birthdate = userViewModel.Birthdate;
				user.Imgpath = "~/User/img/chef.png";
				await _context.AddAsync(user);
				await _context.SaveChangesAsync();
				userViewModel.UserId = user.Id;

				Login login = new Login();
				login.Email = userViewModel.Email;
				login.UserName = userViewModel.UserName;
				login.Password = userViewModel.Password;
				login.UserId = userViewModel.UserId;
				await _context.AddAsync(login);
				await _context.SaveChangesAsync();
				return RedirectToAction("Login", "Auth");
			}
			return View();

		}
		public IActionResult CompleteRegister()
		{
			return View();
		}
		[HttpPost]
		public IActionResult CompleteRegister(UserViewModel userViewModel,decimal id)
		{
            if (ModelState.IsValid) {
                User user = new User();
				user.Birthdate = userViewModel.Birthdate;
				_context.AddAsync(user);
				_context.SaveChanges();
				return RedirectToAction("Index", "Home");
			}
			return View();
		}
		public IActionResult ForgotPassword()
        {
            return View();
        }
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
		
			return View(nameof(Login));
		}
        //public IActionResult UpdateProfile(decimal id, [Bind("Id,FirstName,Lastname,Email,Username,Password")]UpdateProfile updateProfile)
        //{
          

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(recipe);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RecipeExists(recipe.Id))
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
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", recipe.CategoryId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recipe.UserId);
        //    return View(recipe);
    }
}
