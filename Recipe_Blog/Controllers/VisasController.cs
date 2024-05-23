using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recipe_Blog.Models;

namespace Recipe_Blog.Controllers
{
    public class VisasController : Controller
    {
        private readonly ModelContext _context;

        public VisasController(ModelContext context)
        {
            _context = context;
        }

        

        // GET: Visas/Details/5
        public async Task<IActionResult> CardDetails(decimal? id)
        {
            if (id == null || _context.Visas == null)
            {
                return NotFound();
            }

            var visa = await _context.Visas
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visa == null)
            {
                return NotFound();
            }

            return View(visa);
        }

        // GET: Visas/Create
        public IActionResult CreateCard()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Visas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCard([Bind("Id,Cardnumber,Cvc,Nameoncard,Amount,UserId,Expdate")] Visa visa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", visa.UserId);
            return View(visa);
        }

        // GET: Visas/Edit/5
        public async Task<IActionResult> EditCard(decimal? id)
        {
            if (id == null || _context.Visas == null)
            {
                return NotFound();
            }

            var visa = await _context.Visas.FindAsync(id);
            if (visa == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", visa.UserId);
            return View(visa);
        }

        // POST: Visas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCard(decimal id, [Bind("Id,Cardnumber,Cvc,Nameoncard,Amount,UserId,Expdate")] Visa visa)
        {
            if (id != visa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisaExists(visa.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", visa.UserId);
            return View(visa);
        }

        // GET: Visas/Delete/5
        public async Task<IActionResult> DeleteCard(decimal? id)
        {
            if (id == null || _context.Visas == null)
            {
                return NotFound();
            }

            var visa = await _context.Visas
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visa == null)
            {
                return NotFound();
            }

            return View(visa);
        }

        // POST: Visas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCardConfirmed(decimal id)
        {
            if (_context.Visas == null)
            {
                return Problem("Entity set 'ModelContext.Visas'  is null.");
            }
            var visa = await _context.Visas.FindAsync(id);
            if (visa != null)
            {
                _context.Visas.Remove(visa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisaExists(decimal id)
        {
          return (_context.Visas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
