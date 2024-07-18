using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ahlia.Models;
using Nest;
using Microsoft.AspNetCore.Authorization;

namespace Ahlia.Controllers
{
   
    public class StockPricesController : Controller
    {
        private readonly AhliahContext _context;

        public StockPricesController(AhliahContext context)
        {
            _context = context;
        }

        // GET: StockPrices
       
        public async Task<IActionResult> Index()
        {
              return View(await _context.StockPrices.ToListAsync());
        }

        // GET: StockPrices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StockPrices == null)
            {
                return NotFound();
            }

            var stockPrice = await _context.StockPrices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockPrice.ContractImage != null)
            {
                string imageBase64Data =
  Convert.ToBase64String(stockPrice.ContractImage);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                //ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl = imageDataURL;
            }

            if (stockPrice == null)
            {
                return NotFound();
            }

            return View(stockPrice);
        }

        // GET: StockPrices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StockPrices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        public async Task<IActionResult> Create([Bind("Id,Shareprice,Year,IsApprove,IsDeleted,ContractImage,Sharedate")] StockPrice stockPrice, IFormFile? ContractImage)
        {
            
            //isactive: يعني  يوجد أسهم
            var client = _context.Clients.Where(i => i.IsActive == true).ToList();
           



            if (ModelState.IsValid)
            {
                stockPrice.Year = stockPrice.Sharedate.Value.Year;
                var allstocks=_context.StockPrices.Where(s=>s.Year == stockPrice.Year).ToList();
                if (allstocks.Any())
                {
                    ViewBag.message = "لقد قمت بتوزيع الأرباح لهذه السنة من قبل ";
                    return View(stockPrice);
                }
                else
               { 
                    if (ContractImage != null)
                    {

                        MemoryStream ms = new MemoryStream();
                        ContractImage.CopyTo(ms);
                        stockPrice.ContractImage = ms.ToArray();


                        ms.Close();
                        ms.Dispose();
                    }


                    //stockPrice.Sharedate = DateTime.Now;
                    _context.Add(stockPrice);
                    _context.SaveChanges();
                    var StokId = stockPrice.Id;
                    for (int i = 0; i < client.Count; i++)
                    {

                        Penefit penefit = new Penefit();

                        penefit.PriceId = StokId;
                        penefit.ClientId = client[i].Id;
                        var TotalStock = client[i].ActiveStocks + client[i].NotactiveStocks;
                        penefit.CompleteAmount = TotalStock * stockPrice.Shareprice;
                        _context.Add(penefit);
                        _context.SaveChanges();

                    }
                    return RedirectToAction(nameof(Index)); }
            }
            return View(stockPrice);
        }
        // GET: StockPrices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StockPrices == null)
            {
                return NotFound();
            }

            var stockPrice = await _context.StockPrices.FindAsync(id);
            if (stockPrice == null)
            {
                return NotFound();
            }
            return View(stockPrice);
        }

        // POST: StockPrices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Shareprice,Sharedate,Year,IsApprove,IsDeleted,ContractImage")] StockPrice stockPrice, IFormFile? ContractImage)
        {
            if (id != stockPrice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //var allstocks = _context.StockPrices.Where(s => s.Year == stockPrice.Year).ToList();

                    //if (allstocks.Any())
                    //{
                    //    ViewBag.message = "لقد قمت بتوزيع الأرباح لهذه السنة من قبل ";
                    //    return View(stockPrice);
                    //}
                  //  else
                    //{
                        var penfits = _context.Penefits.Where(p => p.PriceId == id).Include(c => c.Client).ToList();
                        if (ContractImage != null)
                        {



                            MemoryStream ms = new MemoryStream();
                            ContractImage.CopyTo(ms);
                            stockPrice.ContractImage = ms.ToArray();

                            ms.Close();
                            ms.Dispose();


                            _context.Update(stockPrice);
                            await _context.SaveChangesAsync();
                            for (int i = 0; i < penfits.Count; i++)
                            {



                                var TotalStock = penfits[i].Client.ActiveStocks + penfits[i].Client.NotactiveStocks;
                                penfits[i].CompleteAmount = TotalStock * stockPrice.Shareprice;

                            }
                            _context.UpdateRange(penfits);
                            _context.SaveChanges();

                        }
                        else
                        {
                            var existing = _context.StockPrices.Find(id);
                            existing.Shareprice = stockPrice.Shareprice;
                            existing.Sharedate = stockPrice.Sharedate;
                            existing.Year = stockPrice.Year;
                            existing.IsApprove = stockPrice.IsApprove;
                            existing.IsDeleted = stockPrice.IsDeleted;
                            _context.Update(existing);
                            await _context.SaveChangesAsync();
                            for (int i = 0; i < penfits.Count; i++)
                            {



                                var TotalStock = penfits[i].Client.ActiveStocks + penfits[i].Client.NotactiveStocks;
                                penfits[i].CompleteAmount = TotalStock * stockPrice.Shareprice;

                            }
                            _context.UpdateRange(penfits);
                            _context.SaveChanges();

                        }
                   // }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockPriceExists(stockPrice.Id))
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
            return View(stockPrice);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StockPrices == null)
            {
                return NotFound();
            }

            var stockPrice = await _context.StockPrices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockPrice == null)
            {
                return NotFound();
            }

            return View(stockPrice);
        }

        // POST: StockPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StockPrices == null)
            {
                return Problem("Entity set 'AhliahContext.StockPrices'  is null.");
            }
            var stockPrice = await _context.StockPrices.FindAsync(id);


            //var stockpenfits = _context.Penefits.Where(r => r.PriceId == stockPrice.Id).ToList();
            //if (stockpenfits.Any())
            //{
            //    _context.Penefits.RemoveRange(stockpenfits);
            //    await _context.SaveChangesAsync();
            //}
            if (stockPrice != null)
            {
                _context.StockPrices.Remove(stockPrice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockPriceExists(int id)
        {
          return _context.StockPrices.Any(e => e.Id == id);
        }
    }
}
