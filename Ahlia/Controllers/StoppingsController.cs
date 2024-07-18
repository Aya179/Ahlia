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
    public class StoppingsController : Controller
    {
        private readonly AhliahContext _context;

        public StoppingsController(AhliahContext context)
        {
            _context = context;
        }

        // GET: Stoppings
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Stoppings.ToListAsync());
        }

        // GET: Stoppings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stoppings == null)
            {
                return NotFound();
            }

            var stopping = await _context.Stoppings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stopping == null)
            {
                return NotFound();
            }

            return View(stopping);
        }

        // GET: Stoppings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stoppings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StoppedStatus")] Stopping stopping)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stopping);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stopping);
        }

        // GET: Stoppings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stoppings == null)
            {
                return NotFound();
            }

            var stopping = await _context.Stoppings.FindAsync(id);
            if (stopping == null)
            {
                return NotFound();
            }
            return View(stopping);
        }

        // POST: Stoppings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StoppedStatus")] Stopping stopping)
        {
            if (id != stopping.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stopping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoppingExists(stopping.Id))
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
            return View(stopping);
        }

        // GET: Stoppings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stoppings == null)
            {
                return NotFound();
            }

            var stopping = await _context.Stoppings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stopping == null)
            {
                return NotFound();
            }

            return View(stopping);
        }

        // POST: Stoppings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stoppings == null)
            {
                return Problem("Entity set 'AhliahContext.Stoppings'  is null.");
            }
            var stopping = await _context.Stoppings.FindAsync(id);
            if (stopping != null)
            {
                _context.Stoppings.Remove(stopping);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoppingExists(int id)
        {
          return _context.Stoppings.Any(e => e.Id == id);
        }
    }
}
