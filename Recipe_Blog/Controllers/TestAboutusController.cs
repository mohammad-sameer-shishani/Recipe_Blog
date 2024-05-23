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
    public class TestAboutusController : Controller
    {
        private readonly ModelContext _context;

        public TestAboutusController(ModelContext context)
        {
            _context = context;
        }

      

        // GET: TestAboutus/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Aboutus == null)
            {
                return NotFound();
            }

            var aboutu = await _context.Aboutus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutu == null)
            {
                return NotFound();
            }

            return View(aboutu);
        }

        // GET: TestAboutus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestAboutus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AboutusContent,AboutCreator,Id,Ingpath")] Aboutu aboutu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aboutu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutu);
        }

        // GET: TestAboutus/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Aboutus == null)
            {
                return NotFound();
            }

            var aboutu = await _context.Aboutus.FindAsync(id);
            if (aboutu == null)
            {
                return NotFound();
            }
            return View(aboutu);
        }

        // POST: TestAboutus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("AboutusContent,AboutCreator,Id,Ingpath")] Aboutu aboutu)
        {
            if (id != aboutu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutuExists(aboutu.Id))
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
            return View(aboutu);
        }

        // GET: TestAboutus/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Aboutus == null)
            {
                return NotFound();
            }

            var aboutu = await _context.Aboutus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutu == null)
            {
                return NotFound();
            }

            return View(aboutu);
        }

        // POST: TestAboutus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Aboutus == null)
            {
                return Problem("Entity set 'ModelContext.Aboutus'  is null.");
            }
            var aboutu = await _context.Aboutus.FindAsync(id);
            if (aboutu != null)
            {
                _context.Aboutus.Remove(aboutu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutuExists(decimal id)
        {
          return (_context.Aboutus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
