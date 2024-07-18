using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ahlia.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ahlia.Controllers
{
    [Authorize]
    public class BanningsController : Controller
    {
        private readonly AhliahContext _context;

        public BanningsController(AhliahContext context)
        {
            _context = context;
        }

        // GET: Bannings
     
        public async Task<IActionResult> Index()
        {
              return View(await _context.Bannings.ToListAsync());
        }

        // GET: Bannings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bannings == null)
            {
                return NotFound();
            }

            var banning = await _context.Bannings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banning == null)
            {
                return NotFound();
            }

            return View(banning);
        }

        // GET: Bannings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bannings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BannedType")] Banning banning)
        {
            if (ModelState.IsValid)
            {
                _context.Add(banning);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(banning);
        }

        // GET: Bannings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bannings == null)
            {
                return NotFound();
            }

            var banning = await _context.Bannings.FindAsync(id);
            if (banning == null)
            {
                return NotFound();
            }
            return View(banning);
        }

        // POST: Bannings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BannedType")] Banning banning)
        {
            if (id != banning.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banning);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BanningExists(banning.Id))
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
            return View(banning);
        }

        // GET: Bannings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bannings == null)
            {
                return NotFound();
            }

            var banning = await _context.Bannings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banning == null)
            {
                return NotFound();
            }

            return View(banning);
        }

        // POST: Bannings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bannings == null)
            {
                return Problem("Entity set 'AhliahContext.Bannings'  is null.");
            }
            var banning = await _context.Bannings.FindAsync(id);
            if (banning != null)
            {
                _context.Bannings.Remove(banning);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BanningExists(int id)
        {
          return _context.Bannings.Any(e => e.Id == id);
        }
        
    }
}
