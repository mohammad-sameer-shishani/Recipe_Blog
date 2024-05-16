using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recipe_Blog.Models;
using Request = Recipe_Blog.Models.Request;

namespace Recipe_Blog.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ModelContext _context;

        public RequestsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Requests.Include(r => r.Recipe).Include(r => r.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Recipe)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {








            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Purchase purchase)
        {
            if (!ModelState.IsValid)
            {
                return View(purchase);
            }

            var userData = await _context.Users.Include(v=>v.Visas).SingleOrDefaultAsync();
            var visaData = _context.Visas.SingleOrDefaultAsync(check=>check.Id == purchase.UserId);
            if (visaData == null) return NotFound();
            //ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", request.RecipeId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", request.UserId);
            foreach (var i in userData.Visas) {
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
                if (i.Expdate != purchase.Expdate)
                {
                    ModelState.AddModelError("", "your Expire date number is invalid");
                    return View(purchase);
                }
                if (i.Nameoncard.ToLower() != purchase.Nameoncard.ToLower().Trim())
                {
                    ModelState.AddModelError("", "your Name is invalid");
                    return View(purchase);
                }
            }
            Request request = new ();
            request.UserId = purchase.UserId;
            request.RecipeId = purchase.RecipeId;
            request.Requestdate = DateTime.Now;
            _context.Add(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", request.RecipeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", request.UserId);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Requestdate,RecipeId,UserId")] Request request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", request.RecipeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", request.UserId);
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Recipe)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'ModelContext.Requests'  is null.");
            }
            var request = await _context.Requests.FindAsync(id);
            if (request != null)
            {
                _context.Requests.Remove(request);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(decimal id)
        {
          return (_context.Requests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
