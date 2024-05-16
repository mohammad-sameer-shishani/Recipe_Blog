using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Recipe_Blog.Models;
using SQLitePCL;

namespace Recipe_Blog.Controllers
{
    public class Request1Controller : Controller
    {
        private readonly ModelContext _context;

        public Request1Controller(ModelContext context)
        {
            _context = context;
        }

        // GET: RequestController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RequestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RequestController/SuccesfullyPurchaced
        public ActionResult SuccesfullyPurchaced()
        {
            return View();
        }

        // POST: RequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRequest(Visa visa,decimal? userId)//##add double tax
        {
            
            if (_context.Visas.SingleOrDefaultAsync(v => v.UserId == userId)!=null)
            {
                //Request request = new Request();
                //request.UserId = UserId;
                //request.RecipeId = RecipeId;
                ////request.Tax = tax;
                //_context.Requests.Add(request);
                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        // GET: RequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: RequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
