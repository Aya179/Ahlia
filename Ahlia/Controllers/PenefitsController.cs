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
   
    public class PenefitsController : Controller
    {
       
        private readonly AhliahContext _context;

        public PenefitsController(AhliahContext context)
        {
            _context = context;
        }

        // GET: Penefits
        
        public async Task<IActionResult> Index()
        {
            var ahliahContext = _context.Penefits.Where(c=>c.CompleteAmount>0)
                .Include(p => p.Client).Include(p => p.Price);
            return View(await ahliahContext.ToListAsync());
        }


        public async Task<IActionResult> getPenfits(int clientId,int stockId)
        {
            var client = _context.Clients.Where(c=>c.Id==clientId).FirstOrDefault();
            int activeStocks = (int)client.ActiveStocks;
            var currentStock=_context.StockPrices.Where(c=>c.Id==stockId).FirstOrDefault();
            decimal price = (decimal)currentStock.Shareprice;
            decimal penfits=price*activeStocks;
            return Json(penfits);   
                }

        // GET: Penefits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Penefits == null)
            {
                return NotFound();
            }

            var penefit = await _context.Penefits
                .Include(p => p.Client)
                .Include(p => p.Price)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (penefit == null)
            {
                return NotFound();
            }

            return View(penefit);
        }

        // GET: Penefits/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FirstName");
            ViewData["PriceId"] = new SelectList(_context.StockPrices, "Id", "Shareprice");
            return View();
        }

        // POST: Penefits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,PriceId,CompleteAmount")] Penefit penefit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(penefit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FirstName", penefit.ClientId);
            ViewData["PriceId"] = new SelectList(_context.StockPrices, "Id", "Shareprice", penefit.PriceId);
            return View(penefit);
        }

        // GET: Penefits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Penefits == null)
            {
                return NotFound();
            }

            var penefit = await _context.Penefits.FindAsync(id);
            if (penefit == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FirstName", penefit.ClientId);
            ViewData["PriceId"] = new SelectList(_context.StockPrices, "Id", "Shareprice", penefit.PriceId);
            return View(penefit);
        }

        // POST: Penefits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,PriceId,CompleteAmount")] Penefit penefit)
        {
            if (id != penefit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(penefit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PenefitExists(penefit.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FirstName", penefit.ClientId);
            ViewData["PriceId"] = new SelectList(_context.StockPrices, "Id", "Shareprice", penefit.PriceId);
            return View(penefit);
        }

        // GET: Penefits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Penefits == null)
            {
                return NotFound();
            }

            var penefit = await _context.Penefits
                .Include(p => p.Client)
                .Include(p => p.Price)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (penefit == null)
            {
                return NotFound();
            }

            return View(penefit);
        }

        // POST: Penefits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Penefits == null)
            {
                return Problem("Entity set 'AhliahContext.Penefits'  is null.");
            }
            var penefit = await _context.Penefits.FindAsync(id);
            if (penefit != null)
            {
                _context.Penefits.Remove(penefit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PenefitExists(int id)
        {
          return _context.Penefits.Any(e => e.Id == id);
        }

        public async Task<IActionResult> CustomerspenfitsIndex()
        {

            return View();
        }
        public async Task<IActionResult> Customerspenfits()
        {


            var sub = from od in _context.Penefits
                      join q in _context.Clients on od.ClientId equals q.Id
                      //join price in _context.pr on od.ClientId equals q.Id
                      // where q.CustomerId == customerId
                      group new { od } by new { od.ClientId, }
                          into v
                      select new
                      {
                          cId = v.Key.ClientId,


                          value = v.Sum(x => x.od.CompleteAmount)

                      };
            var y = sub.ToList();
            var query = from client in _context.Clients
                        join vs in sub on client.Id equals vs.cId
                        //join branch in _context.Branches on vs.bId equals branch.BranchId
                        // join room in _context.QutationRoom on id equals room.id



                        select new
                        {
                            clientId = client.clientnumber,
                            total = vs.value,
                            customername = client.FirstName ,
                            clientid1=client.Id,
                            
                        }
                        ;
            var x = query.ToList();






            return Json(query);

        }

        public async Task<IActionResult> yearpenfitsIndex()
        {

            return View();
        }
        public async Task<IActionResult> yearspenfits()
        {


            var sub = from od in _context.Penefits
                      join q in _context.StockPrices on od.PriceId equals q.Id
                      //join price in _context.pr on od.ClientId equals q.Id
                      // where q.CustomerId == customerId
                      group new { od, q } by new { q.Year }
                          into v
                      select new
                      {
                          cId = v.Key.Year,


                          value = v.Sum(x => x.od.CompleteAmount)

                      };
            var y = sub.ToList();
            





            return Json(y);

        }



        public async Task<IActionResult> detailsClientPenefitsPerYearView(int clientId)
        {
            return View();
        }
        public async Task<IActionResult> detailsClientPenefitsPerYear(int clientId)
        {
            var sub = from od in _context.Penefits
                      join q in _context.Clients on od.ClientId equals q.Id
                      join p in _context.StockPrices on od.PriceId equals p.Id

                       where q.Id == clientId
                      group new { od,p } by new { od.ClientId,p.Year }
                          into v
                      select new
                      {
                          cId = v.Key.ClientId,
                          
                          year= v.Key.Year,
                          value = v.Sum(x => x.od.CompleteAmount)

                      };
            var y = sub.ToList();
            var query = from client in _context.Clients
                        join vs in sub on client.Id equals vs.cId
                       



                        select new
                        {
                            year = vs.year,
                            total = vs.value,
                            customername = client.FirstName ,
                        }
                        ;
            var x = query.ToList();






            return Json(query);

        }
    }
}
