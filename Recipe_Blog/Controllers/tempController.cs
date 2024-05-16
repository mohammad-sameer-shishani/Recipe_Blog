using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Recipe_Blog.Controllers
{
    public class tempController : Controller
    {
        // GET: tempController
        public ActionResult Index()
        {
            return View();
        }

        // GET: tempController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: tempController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tempController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: tempController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: tempController/Edit/5
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

        // GET: tempController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: tempController/Delete/5
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
