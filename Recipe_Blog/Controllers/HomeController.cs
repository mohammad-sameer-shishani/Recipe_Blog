using Microsoft.AspNetCore.Mvc;
using Recipe_Blog.Models;

namespace Recipe_Blog.Controllers
{
	public class HomeController : Controller
	{
		private readonly ModelContext _context;

        public HomeController(ModelContext context)
        {
			_context = context;
        }

        public IActionResult Index()
		{
			var id = HttpContext.Session.GetInt32("userSession");
			var user=_context.Users.SingleOrDefault(x => x.Id == id);
			//ViewBag.thisUser=user;
			return View(user);
		}
	}
}
