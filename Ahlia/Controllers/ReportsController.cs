using Ahlia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nest;
using System.Security.Cryptography.X509Certificates;

namespace Ahlia.Controllers
{
    public class ReportsController : Controller
    {


        private readonly ILogger<ReportsController> _logger;
        private readonly AhliahContext _context;

        public ReportsController(ILogger<ReportsController> logger, AhliahContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult TotalPenfitsPerYearView()
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


        public IActionResult TOp5ClientsView()
        {
            return View();
        }

        public IActionResult TOp5Clients(DateTime startDate, DateTime EndDate)
        {


            var sub = from od in _context.Penefits
                      join q in _context.Clients on od.ClientId equals q.Id
                       join p in _context.StockPrices on od.PriceId equals p.Id

                      //join price in _context.pr on od.ClientId equals q.Id
                      where q.Id == od.ClientId && p.Sharedate >= startDate && p.Sharedate <= EndDate
                      group new { od, q } by new { od.ClientId, q.Id }
                          into v
                      select new
                      {
                          cId = v.Key.ClientId,


                          value = v.Sum(x => x.od.CompleteAmount)

                      };
            var y = sub.ToList();
            var query = from client in _context.Clients
                        join vs in sub on client.Id equals vs.cId




                        select new
                        {
                            //year = vs.year,
                            total = vs.value,
                            customername = client.FirstName,
                        }
                        ;
            var x = query.OrderByDescending(f => f.total)
                 .Take(5)
                 .ToList();






            return Json(x);

        }

        public async Task<IActionResult> ClientsReport()
        {



           




            return View();


        }
        public async Task<IActionResult> ClientsReportApi(DateTime startDate, DateTime EndDate)
        {



            var sub = from od in _context.Penefits
                      join q in _context.Clients on od.ClientId equals q.Id
                      join stock in _context.StockPrices on od.PriceId equals stock.Id
                     // join pay in _context.Payments on q.Id equals pay.ClientId
                      where (stock.Sharedate >= startDate && stock.Sharedate <= EndDate)
                      group new { od } by new { od.ClientId }
                        into v
                      select new
                      {
                          cId = v.Key.ClientId,


                          penefitvalue = v.Sum(x => x.od.CompleteAmount),
                      //  paymentvalue = v.Sum(x => x.pay.Amount)

                          // paymentvalue = v.Sum(x => x.pay.Amount)

                      };
            var sub1 = from pay in _context.Payments
                       join q in _context.Clients on pay.ClientId equals q.Id
                       where pay.Paymentdate >= startDate && pay.Paymentdate <= EndDate

                       group new { pay } by new { pay.ClientId }
                        into v
                       select new
                       {
                           clientId = v.Key.ClientId,


                           paymentvalue = v.Sum(x => x.pay.Amount)

                       };
            var y = sub.ToList();
            var y1 = sub1.ToList();
            var query = from client in _context.Clients
                        join vs in sub on client.Id equals vs.cId
                       // join vs1 in sub1 on client.Id equals vs1.clientId



                        select new selectModel
                        {
                            Id = client.Id,
                          //  total = vs.penefitvalue - vs.paymentvalue,
                            FirstName = client.FirstName,
                           totalPenifet=vs.penefitvalue,
                          // totalPayment=vs.paymentvalue,
                           
                           
                        }
                        ;
            var x = query.ToList();






            return Json(query);


        }

        public IActionResult paymentFromDateToDateReport()
        {
            return View();

        }
        public IActionResult paymentFromDateToDateApi(DateTime startDate, DateTime EndDate)
        {


            var ahliahContext = _context.Payments.Where(c => c.Amount > 0 && c.Paymentdate >= startDate && c.Paymentdate <= EndDate)
               .Include(p => p.CheckerNavigation).Include(p => p.City).Include(p => p.Client).Include(p => p.EditorNavigation);
            return Json(ahliahContext);

        }

        public IActionResult paymentByDateReport()
        {
            return View();

        }
        public IActionResult paymentByDateApi(DateTime startDate)
        {


            var ahliahContext = _context.Payments.Where(c => c.Amount > 0 && c.Paymentdate == startDate )
               .Include(p => p.CheckerNavigation).Include(p => p.City).Include(p => p.Client).Include(p => p.EditorNavigation);
            return Json(ahliahContext);

        }
        public IActionResult PymentByIdReport()
        {
            return View();
        }
        public IActionResult PymentByIdReportApi( int PaymentId)
        {
            var payment = _context.Payments.Include(p=>p.Client)
                .Where(p => p.Id >= PaymentId).ToList();
            
            
            return Json(payment);
        }

        public IActionResult clientStockMovView()

        {
            return View();

        }
        public IActionResult clientStockMov(int id)
        {



            var sub = from od in _context.Penefits
                      join q in _context.Clients on od.ClientId equals q.Id
                      where od.ClientId == id
                      group new { od } by new { od.ClientId }
                        into v
                      select new
                      {
                          cId = v.Key.ClientId,


                          penefitvalue = v.Sum(x => x.od.CompleteAmount),
                          // paymentvalue = v.Sum(x => x.pay.Amount)

                      };
            var sub1 = from pay in _context.Payments
                       join q in _context.Clients on pay.ClientId equals q.Id
                       where pay.ClientId == id

                       group new { pay } by new { pay.ClientId }
                        into v
                       select new
                       {
                           clientId = v.Key.ClientId,


                           paymentvalue = v.Sum(x => x.pay.Amount)

                       };
            var y = sub.ToList();
            var y1 = sub1.ToList();
            var query = from client in _context.Clients
                        join vs in sub on client.Id equals vs.cId
                        join vs1 in sub1 on client.Id equals vs1.clientId



                        select new selectModel
                        {
                            total = vs.penefitvalue - vs1.paymentvalue,
                            ActiveStocks = client.ActiveStocks

                        }
                        ;






            var clientstocks = _context.ClientStokcksMovements.Where(x => x.NewClientId == id || x.ClientId == id).Include(x => x.NewClient).Include(x => x.Client).Include(x => x.MovementType).ToList();


            return Json(new { clientstocks = clientstocks, total = query });
        }
       public IActionResult clientPayment(int clientId)
        {
            var clientPayment = _context.Payments.Include(c=>c.Client)
                .Where(p => p.ClientId == clientId&&p.Amount>0).ToList();
            var clientpenefits = _context.Penefits.Include(c => c.Client).Include(p=>p.Price)
              .Where(p => p.ClientId == clientId&&p.PriceId!=null&&p.CompleteAmount>0).ToList();
            decimal TotalPayment = 0;
            decimal TotalPenefit = 0;
            decimal Total = 0;
            foreach (var p in clientPayment)
            {
                TotalPayment += (decimal)p.Amount;
            }
            foreach (var p in clientpenefits)
            {
                TotalPenefit += (decimal)p.CompleteAmount;
            }
            Total = TotalPenefit - TotalPayment;
            var stockMovs = _context.ClientStokcksMovements.Include(c => c.Client).Include(c => c.MovementType).Include(c => c.NewClient).Where(c=>c.ClientId==clientId||c.NewClientId==clientId).ToList();



            return Json(new {clientPayment= clientPayment,clientpenefits= clientpenefits,toltal =Total ,stockMovs=stockMovs});
        }
        public IActionResult clientPaymentView()
        {
            ViewData["Client"] = new SelectList(_context.Clients, "Id", "FirstName");
            return View();
        }
        public IActionResult DynamicReportView()
        {
            var list = _context.Clients.Include(c => c.City).Include(c => c.ClientType).ToList();

            return View(list);
        }
        [HttpGet]
        public IActionResult DynamicReport1(string select)
        {

            List<string> formattedString = new List<string>();
            formattedString = select.Split(",").ToList();








            return Json(formattedString);


        }



    }
}

