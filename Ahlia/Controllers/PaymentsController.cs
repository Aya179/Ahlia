using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ahlia.Models;
using static Nest.JoinField;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using Org.BouncyCastle.Asn1.X509;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Nest;

namespace Ahlia.Controllers
{
   
    public class PaymentsController : Controller
    {
        private readonly AhliahContext _context;

        public PaymentsController(AhliahContext context)
        {
            _context = context;
        }

        // GET: Payments
        
        public async Task<IActionResult> Index()
        {
            var ahliahContext = _context.Payments.Where(c=>c.Amount>0)
                .Include(p => p.CheckerNavigation).Include(p => p.City).Include(p => p.Client).Include(p => p.EditorNavigation);
            return View(await ahliahContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.CheckerNavigation)
                .Include(p => p.City)
                .Include(p => p.Client)
                .Include(p => p.EditorNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["Checker"] = new SelectList(_context.Employees, "Id", "EmployeeName");
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(c=>c.ClientStatus== "Active"||c.ClientStatus== "غيرمودع"), "Id", "FirstName");
            ViewData["Editor"] = new SelectList(_context.Employees, "Id", "EmployeeName");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            var client = _context.Clients.Find(payment.ClientId);
            ViewBag.clientName = client.FirstName;
            if (payment == null)
            {
                return NotFound();
            }


            var penefits = _context.Penefits.Where(i => i.ClientId ==payment.ClientId );
            var allClientPaymnets = _context.Payments.Where(i => i.ClientId == payment.ClientId);
            decimal totalPenfits = 0;
            foreach (var p in penefits)
            {
                totalPenfits += (decimal)p.CompleteAmount;
            }
            decimal totalpen = totalPenfits;
            decimal totalpaymentsAmount = 0;
            foreach (var pay in allClientPaymnets)
            {
                totalpaymentsAmount += (decimal)pay.Amount;
            }
            decimal totalpay = totalpaymentsAmount;

            var total = totalpen - totalpay;
            ViewBag.Total = total+payment.Amount;
           // ViewBag.Total = total;

            ViewData["Checker"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Checker);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", payment.CityId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FirstName", payment.ClientId);
            ViewData["Editor"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Editor);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Paymentdate,RecieveDate,CheckDate
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,ClientId,CityId,BranchName,PayementFor,Editor,Checker,CheckNumber,BankAccount,ReceiverName,ReceiverNumber,ReceiverResidance,Notes,IsDeleted,IsPayed,CardNumber")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var penifit = new Penefit();
                    penifit.CompleteAmount = payment.Amount;
                    penifit.ClientId = payment.ClientId;


                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var penefits1 = _context.Penefits.Where(i => i.ClientId == payment.ClientId);
                var allClientPaymnets1 = _context.Payments.Where(i => i.ClientId == payment.ClientId);
                decimal totalPenfits1 = 0;
                foreach (var p in penefits1)
                {
                    totalPenfits1 += (decimal)p.CompleteAmount;
                }
                decimal totalpen1 = totalPenfits1;
                decimal totalpaymentsAmount1 = 0;
                foreach (var pay in allClientPaymnets1)
                {
                    totalpaymentsAmount1 += (decimal)pay.Amount;
                }
                decimal totalpay1 = totalpaymentsAmount1;

                var total1 = totalpen1 - totalpay1;
                ViewBag.Total = total1;
                return RedirectToAction(nameof(Index));
            }
            ViewData["Checker"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Checker);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", payment.CityId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FirstName", payment.ClientId);
            ViewData["Editor"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Editor);

            var penefits = _context.Penefits.Where(i => i.ClientId == payment.ClientId);
            var allClientPaymnets = _context.Payments.Where(i => i.ClientId == payment.ClientId);
            decimal totalPenfits = 0;
            foreach (var p in penefits)
            {
                totalPenfits += (decimal)p.CompleteAmount;
            }
            decimal totalpen = totalPenfits;
            decimal totalpaymentsAmount = 0;
            foreach (var pay in allClientPaymnets)
            {
                totalpaymentsAmount += (decimal)pay.Amount;
            }
            decimal totalpay = totalpaymentsAmount;

            var total = totalpen - totalpay;
            ViewBag.Total = total;
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.CheckerNavigation)
                .Include(p => p.City)
                .Include(p => p.Client)
                .Include(p => p.EditorNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Payments == null)
            {
                return Problem("Entity set 'AhliahContext.Payments'  is null.");
            }
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }


        public async Task<IActionResult> ClientPayment(int clientId)
        {

            var penefits = _context.Penefits.Where(i => i.ClientId == clientId);
            var allClientPaymnets = _context.Payments.Where(i => i.ClientId == clientId);
            decimal totalPenfits = 0;
            foreach (var p in penefits)
            {
                totalPenfits += (decimal)p.CompleteAmount;
            }
            decimal totalpaymentsAmount = 0;
            foreach (var pay in allClientPaymnets)
            {
                totalpaymentsAmount += (decimal)pay.Amount;
            }
            decimal totalpay = totalpaymentsAmount;
            decimal totalpen = totalPenfits;

            var total = totalpen - totalpay;




            //

            var clientPayment = _context.Payments
                .Where(p => p.ClientId == clientId&&p.Amount>0).Include(p => p.Client);

            //var sub = from od in _context.Penefits
            //          join q in _context.Clients on od.ClientId equals q.Id
            //          join p in _context.Payments on q.Id equals p.ClientId
                     
            //          where q.Id == clientId
            //          group new { od, p } by new { od.ClientId, p.Client.Id }
            //              into v
            //          select new
            //          {

            //              PeniftsValue = v.Sum(x => x.od.CompleteAmount),
            //              PaymentsValue = v.Sum(x => x.p.Amount),


            //          };
            //var y = sub;
            //var endValue = 0;
            //foreach (var item in y)
            //{
            //    endValue += (int)((int)item.PeniftsValue - item.PaymentsValue);
                ViewBag.pay = "إجمالي الدفعات:"+ totalpay + " ";
                ViewBag.pen="إجمالي الأرباح:"+ totalpen + " ";
            //}
            ViewBag.EndValue = "المبلغ المتبقي:"+ total;
            return View(await clientPayment.ToListAsync());

        }




        public async Task<IActionResult> bill(int PaymentId)
        {



            var sub = from od in _context.Penefits
                      join q in _context.Clients on od.ClientId equals q.Id
                      // join pay in _context.Payments on q.Id equals pay.ClientId

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
                        join payment in _context.Payments on vs.cId equals payment.ClientId
                        where payment.Id == PaymentId

                        select new selectModel
                        {
                            ClientId = client.Id,
                            ActiveStocks = client.ActiveStocks,
                            FirstName = client.FirstName,
                            total = vs.penefitvalue - vs1.paymentvalue,
                            Amount = payment.Amount,
                            Paymentdate = payment.Paymentdate,
                            NationalId=client.NationalId,
                            totalbeforpayments=(vs.penefitvalue - vs1.paymentvalue) + payment.Amount,
                            clientnumber=client.clientnumber
                        }
                        ;

            var x = query.First();
            //////////////////////////////////////////////
            //queryForPriceYear:
            var payment2 = _context.Payments.Where(payment => payment.Id == PaymentId).FirstOrDefault();
            var sub2 = from od in _context.Penefits
                       join q in _context.Clients on od.ClientId equals q.Id
                       join p in _context.StockPrices on od.PriceId equals p.Id

                       //join price in _context.pr on od.ClientId equals q.Id
                       where q.Id == payment2.ClientId
                       group new { od, p } by new { od.ClientId, p.Year }
                         into v
                       select new
                       {
                           cId = v.Key.ClientId,

                           year = v.Key.Year,
                           value = v.Sum(x => x.od.CompleteAmount)

                       };
            var y2 = sub2.ToList();
            var query2 = from client in _context.Clients
                         join vs in sub2 on client.Id equals vs.cId
                         //join branch in _context.Branches on vs.bId equals branch.BranchId
                         // join room in _context.QutationRoom on id equals room.id



                         select new
                         {
                             year1 = vs.year,
                         }
                        ;
            var x2 = query2.ToList();
            List<int> yea = new List<int> { };
            foreach (var item in x2)
            {
                yea.Add((int)item.year1);
            }

            ViewBag.year = yea;
            ViewBag.date = payment2.Paymentdate;
            ViewBag.id = PaymentId;
            return View(x);


        }

        public IActionResult ClientData(int clientId)
        {
            var penefits = _context.Penefits.Where(i => i.ClientId == clientId);
            var allClientPaymnets = _context.Payments.Where(i => i.ClientId == clientId);
            decimal totalPenfits = 0;
            foreach (var p in penefits)
            {
                totalPenfits += (decimal)p.CompleteAmount;
            }
            decimal totalpen = totalPenfits;
            decimal totalpaymentsAmount = 0;
            foreach (var pay in allClientPaymnets)
            {
                totalpaymentsAmount += (decimal)pay.Amount;
            }
            decimal totalpay = totalpaymentsAmount;

            var total = totalpen - totalpay;
            var client = _context.Clients.Where(i => i.Id == clientId).FirstOrDefault();
            

            return Json(new { total = total, card = client.BankAccount, add = client.OriginalAddress });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,ClientId,CityId,BranchName,PayementFor,Editor,Checker,CheckNumber,BankAccount,ReceiverName,ReceiverNumber,ReceiverResidance,Notes,IsDeleted,IsPayed,CardNumber")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                var client = _context.Clients.Find(payment.ClientId);
                ViewBag.clientName = client.FirstName;
                var penefits = _context.Penefits.Where(i => i.ClientId == payment.ClientId);
                var allClientPaymnets = _context.Payments.Where(i => i.ClientId == payment.ClientId);
                decimal totalPenfits = 0;
                foreach (var p in penefits)
                {
                    totalPenfits += (decimal)p.CompleteAmount;
                }
                decimal totalpen = totalPenfits;
                decimal totalpaymentsAmount = 0;
                foreach (var pay in allClientPaymnets)
                {
                    totalpaymentsAmount += (decimal)pay.Amount;
                }
                decimal totalpay = totalpaymentsAmount;
                var total = totalpen - totalpay;

                if (total>0)
                {
                    if (payment.Amount <= total)
                    {
                        payment.Paymentdate = DateTime.Now;
                        payment.RecieveDate = DateTime.Now;
                        payment.CheckDate = DateTime.Now;
                        _context.Add(payment);
                         _context.SaveChanges();
                        return RedirectToAction("bill", "Payments", new { PaymentId =payment.Id});
                    }
                    else
                    {
                        ViewBag.msg = "firstelse";
                        ViewData["Checker"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Checker);
                        ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", payment.CityId);
                        ViewData["ClientId"] = new SelectList(_context.Clients.Where(c => c.ClientStatus == "Active" || c.ClientStatus == "غيرمودع"), "Id", "FirstName", payment.ClientId);
                        ViewData["Editor"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Editor);
                        return View(payment);
                    }
                }

                //else
                //{
                //    ViewBag.msg = "secondelse";
                //    ViewData["Checker"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Checker);
                //    ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", payment.CityId);
                //    ViewData["ClientId"] = new SelectList(_context.Clients.Where(c => c.ClientStatus == "Active" || c.ClientStatus == "غيرمودع"), "Id", "FirstName", payment.ClientId);
                //    ViewData["Editor"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Editor);
                //    return View(payment);
                //}
               

            }
            ViewData["Checker"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Checker);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", payment.CityId);
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(c => c.ClientStatus == "Active" || c.ClientStatus == "غيرمودع"), "Id", "FirstName", payment.ClientId);
            ViewData["Editor"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Editor);
            return View(payment);
        }


        public IActionResult CreateClientPayment(int clientId)
        {
            var penefits = _context.Penefits.Where(i => i.ClientId == clientId);
            var allClientPaymnets = _context.Payments.Where(i => i.ClientId == clientId);
            decimal totalPenfits = 0;
            foreach (var p in penefits)
            {
                totalPenfits += (decimal)p.CompleteAmount;
            }
            decimal totalpen = totalPenfits;
            decimal totalpaymentsAmount = 0;
            foreach (var pay in allClientPaymnets)
            {
                totalpaymentsAmount += (decimal)pay.Amount;
            }
            decimal totalpay = totalpaymentsAmount;

            var total = totalpen - totalpay;
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            var client = _context.Clients.Find(clientId);
            ViewBag.clientName = client.FirstName;
            ViewBag.Total = total;
            return View();
        }

        [HttpPost]
        public IActionResult CreateClientPaymentApi(int clintId, decimal Amount, int CityId, string BranchName, string PayementFor, string BankAccount, string ReceiverName, string ReceiverNumber, string Notes)
        {
            var penefits = _context.Penefits.Where(i => i.ClientId == clintId);
            var allClientPaymnets = _context.Payments.Where(i => i.ClientId == clintId);
            decimal totalPenfits = 0;
            foreach (var p in penefits)
            {
                totalPenfits += (decimal)p.CompleteAmount;
            }
            decimal totalpen = totalPenfits;
            decimal totalpaymentsAmount = 0;
            foreach (var pay in allClientPaymnets)
            {
                totalpaymentsAmount += (decimal)pay.Amount;
            }
            decimal totalpay = totalpaymentsAmount;

            //var total = totalpen - totalpay;
            var total = 100;
            Payment payment = new Payment();

            var client = _context.Clients.Where(i => i.Id == clintId).FirstOrDefault();
            if(total>0)
            {
                if(Amount<=total)
                {
                   
                    payment.Paymentdate = DateTime.Now;
                    payment.ClientId = client.Id;
                    payment.Amount = Amount;
                    payment.CityId = CityId;
                    payment.BranchName = BranchName;
                    payment.PayementFor = PayementFor;
                    payment.BankAccount = BankAccount;
                    payment.ReceiverNumber = ReceiverNumber;
                    payment.ReceiverName = ReceiverName;
                    payment.Notes = Notes;
                    _context.Payments.Add(payment);
                    _context.SaveChanges();
                    //ViewBag.Id = payment.Id;
                    return Json(payment.Id);


                }
                else
                {
                    ViewBag.msg = "firstelse";
                    ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
                    return Json("your Amount>total");

                    //return RedirectToAction("CreateClientPayment");
                }
            }
            //var LastPayment = _context.Payments.LastOrDefault();

            //Payment payment = new Payment();
            //payment.Paymentdate = DateTime.Now;
            //payment.ClientId = client.Id;
            //payment.Amount = Amount;
            //payment.CityId = CityId;
            //payment.BranchName = BranchName;
            //payment.PayementFor = PayementFor;
            //payment.BankAccount = BankAccount;
            //payment.ReceiverNumber = ReceiverNumber;
            //payment.ReceiverName = ReceiverName;
            //payment.Notes = Notes;
            //_context.Payments.Add(payment);
            //_context.SaveChanges();
            //ViewBag.PaymentId = LastPayment.Id;
            //var Payment3= _context.Payments
            //  .LastOrDefault(p => p.Id == _context.Payments.Max(x => x.Id));
            //ViewBag.PaymentId = Payment3.Id;
            //  return RedirectToAction("bill", "Payments", new { PaymentId = payment.Id });


            return Json(" your total<0");
        }
      

    }
}
