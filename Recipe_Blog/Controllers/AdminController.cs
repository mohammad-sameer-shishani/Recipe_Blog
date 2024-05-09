using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipe_Blog.Models;

namespace Recipe_Blog.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;

		public AdminController(ModelContext context)
		{
			_context = context;
		}
        
        public IActionResult Index()
		{
            var id = HttpContext.Session.GetInt32("adminSession");//=>5

			var admin = _context.Users.SingleOrDefault(u=>u.Id==id);
			ViewBag.currentAdmin = admin;
            ViewBag.userCount = _context.Users.Where(x=> x.RoleId == 3).Count();
            ViewBag.chefCount = _context.Users.Where(x=> x.RoleId == 2).Count();
            return View();
        }
		public IActionResult Home()
		{
            return View();
        }
        public IActionResult Testimonials()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public async Task<IActionResult> RegisteredUsers()
        {
            var  modelContext = await _context.Logins
                .Include(u => u.User)
                .Include(u => u.User.Role)
                .Where(u => u.User.Role.Roleid == 3).OrderBy(u => u.Id).ToListAsync();
            return View(modelContext);
        }
        public async Task<IActionResult> RegisteredChefs()
        {

            List<Login> modelContext =await _context.Logins
                .Include(c=>c.User)
                .Include(r => r.User.Recipes)
                .Include(r=> r.User.Role)
                .Where(check=> check.User.Role.Roleid == 2)
                .OrderBy(x => x.Id).ToListAsync();
           


            return View(modelContext);
        }
        public IActionResult Profile()
        {
            return View();
        }
    }
}
