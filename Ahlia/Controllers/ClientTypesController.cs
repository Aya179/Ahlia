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
    public class ClientTypesController : Controller
    {
        private readonly AhliahContext _context;

        public ClientTypesController(AhliahContext context)
        {
            _context = context;
        }
        
        // GET: ClientTypes
        public async Task<IActionResult> Index()
        {
              return View(await _context.ClientTypes.ToListAsync());
        }

        // GET: ClientTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClientTypes == null)
            {
                return NotFound();
            }

            var clientType = await _context.ClientTypes
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (clientType == null)
            {
                return NotFound();
            }

            return View(clientType);
        }

        // GET: ClientTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClientTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,TypeName,Notes")] ClientType clientType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientType);
        }

        // GET: ClientTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClientTypes == null)
            {
                return NotFound();
            }

            var clientType = await _context.ClientTypes.FindAsync(id);
            if (clientType == null)
            {
                return NotFound();
            }
            return View(clientType);
        }

        // POST: ClientTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeId,TypeName,Notes")] ClientType clientType)
        {
            if (id != clientType.TypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientTypeExists(clientType.TypeId))
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
            return View(clientType);
        }

        // GET: ClientTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClientTypes == null)
            {
                return NotFound();
            }

            var clientType = await _context.ClientTypes
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (clientType == null)
            {
                return NotFound();
            }

            return View(clientType);
        }

        // POST: ClientTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClientTypes == null)
            {
                return Problem("Entity set 'AhliahContext.ClientTypes'  is null.");
            }
            var clientType = await _context.ClientTypes.FindAsync(id);
            if (clientType != null)
            {
                _context.ClientTypes.Remove(clientType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientTypeExists(int id)
        {
          return _context.ClientTypes.Any(e => e.TypeId == id);
        }
    }
}
