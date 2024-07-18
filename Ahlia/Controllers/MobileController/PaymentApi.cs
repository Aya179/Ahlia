using Ahlia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Nest.JoinField;

namespace Ahlia.Controllers.MobileController
{
    public class PaymentApi : Controller
    {
        private readonly AhliahContext _context;

        public PaymentApi(AhliahContext context)
        {
            _context = context;
        }
        public IActionResult GetAllPayment()
        {
            var Payment = _context.Payments.ToList();
            return Json(Payment);
        }
        public IActionResult GetClient()
        {
            var Client = _context.Clients.Select(c => new
            {
                id = c.Id,
                name = c.FirstName + " " + c.LastName
            });
            return Json(Client);
        }
        public IActionResult GetCheker()
        {
            var checker = _context.Employees.Select(c => new
            {
                id = c.Id,
                name = c.EmployeeName
            });
            return Json(checker);
        }
        public IActionResult GetEditor()
        {
            var Editor = _context.Employees.Select(c => new
            {
                id = c.Id,
                name = c.EmployeeName
            });
            return Json(Editor);
        }

        public IActionResult CreatePayment(int clientId, decimal Amount, string BranchName, string PayementFor, int Editor, int Checker, string CheckNumber, string BankAccount, DateTime CheckDate, string ReceiverName, string ReceiverNumber, DateTime RecieveDate, string ReceiverResidance, string Notes, int CardNumber)
        {
            Payment payment = new Payment();
            payment.ClientId = clientId;
            payment.Amount = Amount;
            payment.BranchName = BranchName;
            payment.PayementFor = PayementFor;
            payment.Editor = Editor;
            payment.Checker = Checker;
            payment.CheckDate = CheckDate;
            payment.ReceiverName = ReceiverName;
            payment.ReceiverNumber = ReceiverNumber;
            payment.CardNumber = CardNumber;
            payment.Notes = Notes;
            payment.RecieveDate = RecieveDate;
            payment.CheckNumber = CheckNumber;
            payment.BankAccount = BankAccount;
            payment.ReceiverResidance = ReceiverResidance;

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
            if (totalpen != totalpay)
            {
                if (payment.Amount <= totalpen)
                {
                    payment.Paymentdate = DateTime.Now;
                    _context.Add(payment);
                    _context.SaveChanges();
                    return Json(payment);
                }
                else
                {
                    return Json("لقد سحبت أكبر من المبلغ الموجود في رصيد الزبون");
                }

            }
            else
            {
                return Json("لا يوجد في رصيدك مبلغ للسحب لقد استهلكت كامل الرصيد");
            }
        }

        public IActionResult DeletePayment(int PaymentId)
        {
            var payment = _context.Payments.Find(PaymentId);
            _context.Remove(payment);
            _context.SaveChanges();
            return Json("تم الحذف بنجاح");

        }
        public IActionResult PaymentDetails(int PaymentId)
        {
            var payment = _context.Payments
                .Include(p => p.CheckerNavigation)
                .Include(p => p.City)
                .Include(p => p.Client)
                .Include(p => p.EditorNavigation)
                .FirstOrDefault(p => p.Id == PaymentId);
            return Json(payment);

        }

        public IActionResult UpdtePayment(int id, int clientId, decimal Amount, string BranchName, string PayementFor, int Editor, int Checker, string CheckNumber, string BankAccount, DateTime CheckDate, string ReceiverName, string ReceiverNumber, DateTime RecieveDate, string ReceiverResidance, string Notes, int CardNumber)
        {
            var payment = _context.Payments.Find(id); 
            if (clientId != null)
            { 
                payment.ClientId = clientId;
            }
            if (Amount != null)
            {
                payment.Amount = Amount;
            }
            if (BranchName != null)
            {
                payment.BranchName = BranchName;
            }
            if (PayementFor != null)
            {
                payment.PayementFor = PayementFor;
            }
            if (Editor != null)
            {
                payment.Editor = Editor;
            }
            if (Checker != null)
            {
                payment.Checker = Checker;
            }
            if (CheckDate != null)
            {
                payment.CheckDate = CheckDate;
            }
            if (ReceiverName != null)
            {
                payment.ReceiverName = ReceiverName;
            }
            if (ReceiverNumber != null)
            {
                payment.ReceiverNumber = ReceiverNumber;
            }
            if (CardNumber != null)
            {
                payment.CardNumber = CardNumber;
            }
            if (Notes != null)
            {
                payment.Notes = Notes;
            }
            if (RecieveDate != null)
            {
                payment.RecieveDate = RecieveDate;
            }
            if (CheckNumber != null)
            {
                payment.CheckNumber = CheckNumber;
            }
            if (BankAccount != null)
            {
                payment.BankAccount = BankAccount;
            }
            if (ReceiverResidance != null)
            {
                payment.ReceiverResidance = ReceiverResidance;
            }
            _context.Update(payment);
            _context.SaveChanges();
            return Json(payment);


        }
    }
}