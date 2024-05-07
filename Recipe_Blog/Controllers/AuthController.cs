using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipe_Blog.Models;

namespace Recipe_Blog.Controllers
{
    public class AuthController : Controller
    {
        private readonly ModelContext _context;
        public AuthController(ModelContext context)
        {
            _context = context;
        }

        public IActionResult ChoosingRole()
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
					HttpContext.Session.SetInt32("adminSession", (int)userId_); //{"adminSession": 5}
                    return RedirectToAction("Index", "Admin");
				case 2: // chef
					HttpContext.Session.SetInt32("chefSession", (int)userId_);
					return RedirectToAction("Create","ChefRecipes");
				case 3: // user/customer
					HttpContext.Session.SetInt32("userSession", (int)userId_);
                    return RedirectToAction("Index", "Home");
				default:
					ModelState.AddModelError("","Something Error");
					return View(login);
			}

          
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(UserViewModel userViewModel, decimal Roleid)
        {

            if (ModelState.IsValid)
            {
                User user = new User();
                user.Firstname = userViewModel.Firstname;
                user.Lastname = userViewModel.Lastname;
                user.RoleId = Roleid;
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
	}
}
