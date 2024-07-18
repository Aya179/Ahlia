using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ahlia.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using Nest;

namespace Ahlia.Controllers
{
    public class RassingsController : Controller
    {
        private readonly AhliahContext _context;

        public RassingsController(AhliahContext context)
        {
            _context = context;
        }

        // GET: Rassings
        public async Task<IActionResult> Index()
        {
              return _context.Rassing != null ? 
                          View(await _context.Rassing.ToListAsync()) :
                          Problem("Entity set 'AhliahContext.Rassing'  is null.");
        }

        // GET: Rassings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rassing == null)
            {
                return NotFound();
            }

            var rassing = await _context.Rassing
                .FirstOrDefaultAsync(m => m.id == id);
            if (rassing.ContractImag != null)
            {
                string imageBase64Data =
  Convert.ToBase64String(rassing.ContractImag);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                //ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl = imageDataURL;
            }
            if (rassing == null)
            {
                return NotFound();
            }

            return View(rassing);
        }

        // GET: Rassings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rassings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Amount,ContractImag,RassingType")] Rassing rassing  ,IFormFile? ContractImage)
        {
            var client = _context.Clients.ToList();
            if (ModelState.IsValid)
            {

                if (ContractImage != null)
                {

                    MemoryStream ms = new MemoryStream();
                    ContractImage.CopyTo(ms);
                    rassing.ContractImag = ms.ToArray();


                    ms.Close();
                    ms.Dispose();
                }
                _context.Add(rassing);
                var rassingid = rassing.id;
                for (int i = 0; i < client.Count; i++)
                {
                    client[i].ActiveStocks = (int?)(client[i].ActiveStocks *rassing.Amount);
                    client[i].NotactiveStocks = (int?)(client[i].NotactiveStocks * rassing.Amount);


                    _context.Update(client[i]);
                     _context.SaveChanges();

                }
                rassing.RassingDate = DateTime.Now;
                 _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(rassing);
        }

        // GET: Rassings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rassing == null)
            {
                return NotFound();
            }

            var rassing = await _context.Rassing.FindAsync(id);
            if (rassing == null)
            {
                return NotFound();
            }
            return View(rassing);
        }

        // POST: Rassings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Amount,ContractImag")] Rassing rassing, IFormFile? ContractImage)
        {
            if (id != rassing.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ContractImage != null)
                    {



                        MemoryStream ms = new MemoryStream();
                        ContractImage.CopyTo(ms);
                        rassing.ContractImag = ms.ToArray();

                        ms.Close();
                        ms.Dispose();

                        _context.Update(rassing);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var existing = _context.Rassing.Find(id);
                        existing.Amount = rassing.Amount;
                       
                       
                        _context.Update(existing);
                        await _context.SaveChangesAsync();
                    }

                    //_context.Update(rassing);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RassingExists(rassing.id))
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
            return View(rassing);
        }

        // GET: Rassings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rassing == null)
            {
                return NotFound();
            }

            var rassing = await _context.Rassing
                .FirstOrDefaultAsync(m => m.id == id);
            if (rassing.ContractImag != null)
            {
                string imageBase64Data =
  Convert.ToBase64String(rassing.ContractImag);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                //ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl = imageDataURL;
            }
            if (rassing == null)
            {
                return NotFound();
            }

            return View(rassing);
        }

        // POST: Rassings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rassing == null)
            {
                return Problem("Entity set 'AhliahContext.Rassing'  is null.");
            }
            var client = _context.Clients.ToList();

            var rassing = await _context.Rassing.FindAsync(id);
            if (rassing != null)
            {
                for (int i = 0; i < client.Count; i++)
                {
                    client[i].ActiveStocks = (int?)(client[i].ActiveStocks / rassing.Amount);
                    client[i].NotactiveStocks = (int?)(client[i].NotactiveStocks / rassing.Amount);


                    _context.Update(client[i]);
                    _context.SaveChanges();

                }

                _context.Rassing.Remove(rassing);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RassingExists(int id)
        {
          return (_context.Rassing?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
