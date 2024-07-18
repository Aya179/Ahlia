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
    public class StocksMovementsController : Controller
    {
        private readonly AhliahContext _context;

        public StocksMovementsController(AhliahContext context)
        {
            _context = context;
        }

        // GET: StocksMovements
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return View(await _context.StocksMovements.ToListAsync());
        }

        // GET: StocksMovements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StocksMovements == null)
            {
                return NotFound();
            }

            var stocksMovement = await _context.StocksMovements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stocksMovement == null)
            {
                return NotFound();
            }

            return View(stocksMovement);
        }

        // GET: StocksMovements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StocksMovements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovementType,Description")] StocksMovement stocksMovement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stocksMovement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stocksMovement);
        }

        // GET: StocksMovements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StocksMovements == null)
            {
                return NotFound();
            }

            var stocksMovement = await _context.StocksMovements.FindAsync(id);
            if (stocksMovement == null)
            {
                return NotFound();
            }
            return View(stocksMovement);
        }

        // POST: StocksMovements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovementType,Description")] StocksMovement stocksMovement)
        {
            if (id != stocksMovement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stocksMovement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StocksMovementExists(stocksMovement.Id))
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
            return View(stocksMovement);
        }

        // GET: StocksMovements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StocksMovements == null)
            {
                return NotFound();
            }

            var stocksMovement = await _context.StocksMovements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stocksMovement == null)
            {
                return NotFound();
            }

            return View(stocksMovement);
        }

        // POST: StocksMovements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StocksMovements == null)
            {
                return Problem("Entity set 'AhliahContext.StocksMovements'  is null.");
            }
            var stocksMovement = await _context.StocksMovements.FindAsync(id);
            if (stocksMovement != null)
            {
                _context.StocksMovements.Remove(stocksMovement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StocksMovementExists(int id)
        {
          return _context.StocksMovements.Any(e => e.Id == id);
        }
    }
}
