using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recipe_Blog.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
			ViewBag.Homepage = _context.Homepages.First();
			return View();
		}
		public IActionResult Login()
        {
			ViewBag.Homepage=_context.Homepages.First();

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
			ViewBag.Homepage = _context.Homepages.First();
			return View();
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegisterAsUser(UserViewModel userViewModel)
        {
			ViewBag.Homepage = _context.Homepages.First();
			if (ModelState.IsValid)
            {
				bool checkEmail = await _context.Logins.AnyAsync(u => u.Email.ToLower() == userViewModel.Email.ToLower());
				if (checkEmail)
				{
					ModelState.AddModelError("Email", "Email already exists.");
					return View(userViewModel);
				}

				bool checkUserName = await _context.Logins.AnyAsync(u => u.UserName.ToLower() == userViewModel.UserName.ToLower());
				if (checkUserName)
				{
					ModelState.AddModelError("Username", "Username already exists.");
					return View(userViewModel);
				}
				User user = new User();
                user.Firstname = userViewModel.Firstname;
                user.Lastname = userViewModel.Lastname;
                user.RoleId = 3;
                user.Birthdate = userViewModel.Birthdate;
				user.Imgpath = "man-default.png";

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



				Visa visa = new Visa {
					UserId = userViewModel.UserId,
					Nameoncard = userViewModel.Firstname + userViewModel.Lastname,
					Amount = 1000,
					Cardnumber = 12341234,
                    Expdate = "1103",
					Cvc = 111,
                };
                await _context.AddAsync(visa);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Auth");
            }

            return View();
            
        }
		//GET: Register as chef
		public IActionResult RegisterAsChef()
		{
			ViewBag.Homepage = _context.Homepages.First();
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegisterAsChef(UserViewModel userViewModel)
		{
			ViewBag.Homepage = _context.Homepages.First();
			if (ModelState.IsValid)
			{
				bool checkEmail = await _context.Logins.AnyAsync(u => u.Email.ToLower() == userViewModel.Email.ToLower());
				if (checkEmail)
				{
					ModelState.AddModelError("Email", "Email already exists.");
					return View(userViewModel);
				}

				bool checkUserName = await _context.Logins.AnyAsync(u => u.UserName.ToLower() == userViewModel.UserName.ToLower());
				if (checkUserName)
				{
					ModelState.AddModelError("Username", "Username already exists.");
					return View(userViewModel);
				}
				User user = new User();
				user.Firstname = userViewModel.Firstname;
				user.Lastname = userViewModel.Lastname;
				user.RoleId = 2;
				user.Birthdate = userViewModel.Birthdate;
				user.Imgpath = "chef.png";
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
			ViewBag.Homepage = _context.Homepages.First();
			HttpContext.Session.Clear();
		
			return View(nameof(Login));
		}
   
    }
}
