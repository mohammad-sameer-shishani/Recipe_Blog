using Microsoft.AspNetCore.Mvc;
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

			var user = _context.Users.SingleOrDefault(u=>u.Id==id);
			ViewBag.currentUser = user;
			return View();
        }
    }
}
