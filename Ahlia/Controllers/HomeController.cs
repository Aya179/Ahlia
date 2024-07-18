using Ahlia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ahlia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AhliahContext _context;

        public HomeController(ILogger<HomeController> logger, AhliahContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            bager();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void bager()
        {
            ViewBag.Clientcount = _context.Clients.Count();
                
                var stocks=_context.StockPrices.Count();
                var ClientActivestocks=_context.Clients.Select(c=>c.ActiveStocks).Sum();
                var ClientnonActivestocks=_context.Clients.Select(c=>c.NotactiveStocks).Sum();
            var amount = _context.Payments.Select(c => c.Amount).Sum();
            ViewBag.Stockcount =  ClientActivestocks + ClientnonActivestocks;


            //string year = DateTime.Now.Year.ToString();

            ViewBag.StoppedClientcount = _context.Clients.Where(c=>c.ClientStatus== "غيرمودع").Count();
            ViewBag.BannedClientscount = amount;
           


        }
      
    }
}