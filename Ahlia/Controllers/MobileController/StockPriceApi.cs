using Ahlia.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ahlia.Controllers.MobileController
{
    public class StockPriceApi : Controller
    {
        private readonly AhliahContext _context;

        public StockPriceApi(AhliahContext context)
        {
            _context = context;
        }
        public IActionResult AddStockPrice(decimal price, DateTime date, int year)
        {
            StockPrice stockPrice = new StockPrice();
            stockPrice.Shareprice = price;
            stockPrice.Sharedate = date;
            stockPrice.Year = year;
            _context.Add(stockPrice);
            _context.SaveChanges();
            var client = _context.Clients.Where(i => i.IsActive == true).ToList();
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
            return Json(stockPrice);
        }
        public IActionResult DeleteStockPrice(int Id)
        {
            var stockPrice = _context.StockPrices.FirstOrDefault(s => s.Id == Id);
            if (stockPrice == null)
            {
                return NotFound();
            }
            else
            {
                _context.StockPrices.Remove(stockPrice);
                return Json("تم الحذف بنجاح");
            }
        }
        public IActionResult DetailsStockPrice(int Id)
        {
            var stockPrice = _context.StockPrices.FirstOrDefault(s => s.Id == Id);
            if (stockPrice == null)
            {
                return NotFound();
            }
            else
            {
                return Json(stockPrice);
            }
        }

        public IActionResult UpdateStockprice(int id, decimal price,DateTime date,int year)
        {
            var stockprice = _context.StockPrices.Where(s => s.Id == id).FirstOrDefault();
            if(stockprice==null)
            {
                return NotFound();
            }
            else
            {
                if (price != null)
                {
                    stockprice.Shareprice = price;
                }
                if (year != null)
                {
                    stockprice.Year = year;
                }
                if (date != null)
                {
                    stockprice.Sharedate = date;
                }
                _context.Update(stockprice);
                _context.SaveChanges();
                return Json(stockprice);
            }
        }
        public IActionResult getAllStockPrice()
        {
            var stockprice = _context.StockPrices.ToList();
            return Json(stockprice);
        }
    }
}
