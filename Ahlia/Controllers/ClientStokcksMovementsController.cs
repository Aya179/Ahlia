using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ahlia.Models;
using Microsoft.AspNetCore.Authorization;
using Nest;
using static Nest.JoinField;

namespace Ahlia.Controllers
{
   
    public class ClientStokcksMovementsController : Controller
    {
        private readonly AhliahContext _context;

        public ClientStokcksMovementsController(AhliahContext context)
        {
            _context = context;
        }
       

        // GET: ClientStokcksMovements
        public async Task<IActionResult> Index()
        {
            var ahliahContext = _context.ClientStokcksMovements.Include(c => c.Client).Include(c => c.MovementType).Include(c => c.NewClient);
            return View(await ahliahContext.ToListAsync());
        }

        // GET: ClientStokcksMovements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClientStokcksMovements == null)
            {
                return NotFound();
            }

            var clientStokcksMovement = await _context.ClientStokcksMovements
                .Include(c => c.Client)
                .Include(c => c.MovementType)
                .Include(c => c.NewClient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientStokcksMovement == null)
            {
                return NotFound();
            }

            return View(clientStokcksMovement);
        }

        // GET: ClientStokcksMovements/Create
        public IActionResult Create()
        {
            ViewData["ClientId1"] = new SelectList(_context.Clients.Where(i=>i.ActiveStocks>0), "Id","FirstName");
            ViewData["MovementTypeId"] = new SelectList(_context.StocksMovements, "Id","MovementType");
            ViewData["NewClientId1"] = new SelectList(_context.Clients, "Id","FirstName");
            return View();
        }

        // POST: ClientStokcksMovements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,MovementTypeId,Amount,NewClientId,MovementDate,ContractImage,Reason,Notes,IsApproved")] ClientStokcksMovement clientStokcksMovement)
        {
            if (ModelState.IsValid)
            {
                var oldclient = _context.Clients.Find(clientStokcksMovement.ClientId);
                var newclient = _context.Clients.Find(clientStokcksMovement.NewClientId);
                var penefits = _context.Penefits.Where(i => i.ClientId == oldclient.Id);
                var allClientPaymnets = _context.Payments.Where(i => i.ClientId == oldclient.Id);
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


                if (clientStokcksMovement.Amount>oldclient.ActiveStocks)
                {
                    ViewBag.msg = "FisrtCondition";
                    ViewBag.oldActivStock = oldclient.ActiveStocks;
                    ViewBag.oldClienName = oldclient.FirstName;
                    ViewBag.NewActivStock = newclient.ActiveStocks;
                    ViewBag.NewClienName = newclient.FirstName;
                    ViewBag.mssg = "لا يمكن نقل عدد أسهم أكبر من عدد الأسهم المتاح";
                    ViewData["ClientId1"] = new SelectList(_context.Clients.Where(i => i.ActiveStocks > 0), "Id", "FirstName", clientStokcksMovement.ClientId);
                    ViewData["MovementTypeId"] = new SelectList(_context.StocksMovements, "Id", "MovementType", clientStokcksMovement.MovementTypeId);
                    ViewData["NewClientId1"] = new SelectList(_context.Clients, "Id", "FirstName", clientStokcksMovement.NewClientId);
                    return View(clientStokcksMovement);
                }
               
                else if(total==0)
                {
                    oldclient.ActiveStocks = oldclient.ActiveStocks - clientStokcksMovement.Amount;
                    if (newclient.ActiveStocks == null)
                        newclient.ActiveStocks = clientStokcksMovement.Amount;
                    else 
                        newclient.ActiveStocks = newclient.ActiveStocks + clientStokcksMovement.Amount;
                    if (oldclient.ActiveStocks==0)
                    {
                        oldclient.IsActive = false;
                    }
                    
                    if (newclient.ActiveStocks > 0 && newclient.IsActive == false)
                    {
                        newclient.IsActive = true;
                    }
                    _context.Clients.Update(oldclient);
                    _context.Clients.Update(newclient);
                   
                    _context.Add(clientStokcksMovement);
                    await _context.SaveChangesAsync();
                    if (oldclient.ActiveStocks != 0)
                    {
                        ViewBag.msg = "firstelse";
                        ViewBag.oldActivStock = oldclient.ActiveStocks;
                        ViewBag.oldClienName = oldclient.FirstName;
                        ViewBag.NewActivStock = newclient.ActiveStocks;
                        ViewBag.NewClienName = newclient.FirstName;
                        ViewBag.mssg = "أصبح عدد الأسهم لدى " + " " + oldclient.FirstName + oldclient.ActiveStocks + "  " + " وأصبح عدد الأسهم لدى" + "  " + newclient.FirstName + "  " + newclient.ActiveStocks;
                        ViewData["ClientId1"] = new SelectList(_context.Clients.Where(i => i.ActiveStocks > 0), "Id", "FirstName");
                        ViewData["MovementTypeId"] = new SelectList(_context.StocksMovements, "Id", "MovementType");
                        ViewData["NewClientId1"] = new SelectList(_context.Clients, "Id", "FirstName");
                        return View();
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.secondmssg = "SecondCondition1";
                    ViewBag.oldActivStock = oldclient.ActiveStocks;
                    ViewBag.oldClienName = oldclient.FirstName;
                    ViewBag.NewActivStock = newclient.ActiveStocks;
                    ViewBag.NewClienName = newclient.FirstName;
                    ViewBag.mssg11 = "لا يمكنك النقل الرصيد لدى المساهم الجديد أكبر من الصفر";
                    ViewData["ClientId1"] = new SelectList(_context.Clients.Where(i => i.ActiveStocks > 0), "Id", "FirstName");
                    ViewData["MovementTypeId"] = new SelectList(_context.StocksMovements, "Id", "MovementType");
                    ViewData["NewClientId1"] = new SelectList(_context.Clients, "Id", "FirstName");
                    return View();

                }


            }
            ViewData["ClientId1"] = new SelectList(_context.Clients, "Id", "FirstName");
            ViewData["MovementTypeId"] = new SelectList(_context.StocksMovements, "Id", "MovementType");
            ViewData["NewClientId1"] = new SelectList(_context.Clients, "Id", "FirstName");
            return View();
        }
        
        // GET: ClientStokcksMovements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClientStokcksMovements == null)
            {
                return NotFound();
            }
            var clientStokcksMovement = await _context.ClientStokcksMovements.FindAsync(id);

            var client = _context.Clients.Find(clientStokcksMovement.ClientId);

            ViewBag.id = client.FirstName;
            var clientnew = _context.Clients.Find(clientStokcksMovement.NewClientId);
            ViewBag.id1 = clientnew.FirstName;
            ViewBag.stocks = client.ActiveStocks + client.NotactiveStocks;

            if (clientStokcksMovement == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(i => i.ClientStatus == "Active"), "Id", "FirstName", clientStokcksMovement.ClientId);
            ViewData["MovementTypeId"] = new SelectList(_context.StocksMovements, "Id", "MovementType", clientStokcksMovement.MovementTypeId);
            ViewData["NewClientId"] = new SelectList(_context.Clients.Where(i => i.ClientStatus == "Active"), "Id", "FirstName", clientStokcksMovement.NewClientId);
            return View(clientStokcksMovement);
        }

        // POST: ClientStokcksMovements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,MovementTypeId,Amount,NewClientId,MovementDate,ContractImage,Reason,Notes,IsApproved")] ClientStokcksMovement clientStokcksMovement)
        {
            var oldmovement = _context.ClientStokcksMovements.Find(id);

            if (id != oldmovement.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var oldclient = _context.Clients.Find(clientStokcksMovement.ClientId);
                    var newclient = _context.Clients.Find(clientStokcksMovement.NewClientId);
                    oldclient.ActiveStocks = oldclient.ActiveStocks + oldmovement.Amount;
                    newclient.ActiveStocks = newclient.ActiveStocks - oldmovement.Amount;
                    _context.Update(oldclient);
                    _context.Update(newclient);
                    _context.SaveChanges();

                    oldclient.ActiveStocks = oldclient.ActiveStocks - clientStokcksMovement.Amount;
                    newclient.ActiveStocks = newclient.ActiveStocks + clientStokcksMovement.Amount;
                    if (oldclient.ActiveStocks == 0)
                    {
                        oldclient.IsActive = false;
                    }

                    if (newclient.ActiveStocks > 0 && newclient.IsActive == false)
                    {
                        newclient.IsActive = true;
                    }

                    _context.Update(oldclient);

                    _context.Update(newclient);

                    oldmovement.ClientId = clientStokcksMovement.ClientId;
                    oldmovement.NewClientId = clientStokcksMovement.NewClientId;
                    oldmovement.Amount = clientStokcksMovement.Amount;
                    oldmovement.MovementTypeId=clientStokcksMovement.MovementTypeId;
                    oldmovement.MovementDate=clientStokcksMovement.MovementDate;
                    oldmovement.ContractImage=clientStokcksMovement.ContractImage;
                    oldmovement.Reason=clientStokcksMovement.Reason;
                    oldmovement.Notes=clientStokcksMovement.Notes;  
                    _context.Update(oldmovement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientStokcksMovementExists(oldmovement.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(i => i.ClientStatus == "Active"), "Id", "FirstName", clientStokcksMovement.ClientId);
            ViewData["MovementTypeId"] = new SelectList(_context.StocksMovements, "Id", "MovementType", clientStokcksMovement.MovementTypeId);
            ViewData["NewClientId"] = new SelectList(_context.Clients.Where(i => i.ClientStatus == "Active"), "Id", "FirstName", clientStokcksMovement.NewClientId);
            return View(clientStokcksMovement);
        }

        // GET: ClientStokcksMovements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClientStokcksMovements == null)
            {
                return NotFound();
            }

            var clientStokcksMovement = await _context.ClientStokcksMovements
                .Include(c => c.Client)
                .Include(c => c.MovementType)
                .Include(c => c.NewClient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientStokcksMovement == null)
            {
                return NotFound();
            }

            return View(clientStokcksMovement);
        }

        // POST: ClientStokcksMovements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClientStokcksMovements == null)
            {
                return Problem("Entity set 'AhliahContext.ClientStokcksMovements'  is null.");
            }
            var clientStokcksMovement = await _context.ClientStokcksMovements.FindAsync(id);
            if (clientStokcksMovement != null)
            {
                var oldmovement = _context.ClientStokcksMovements.Find(id);

                var oldclient = _context.Clients.Find(clientStokcksMovement.ClientId);
                var newclient = _context.Clients.Find(clientStokcksMovement.NewClientId);
                oldclient.ActiveStocks = oldclient.ActiveStocks + oldmovement.Amount;
                newclient.ActiveStocks = newclient.ActiveStocks - oldmovement.Amount;
                _context.Update(oldclient);
                _context.Update(newclient);
                _context.SaveChanges();
                _context.ClientStokcksMovements.Remove(clientStokcksMovement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientStokcksMovementExists(int id)
        {
          return _context.ClientStokcksMovements.Any(e => e.Id == id);
        }

        public IActionResult GetClientStock(int id)
        {
            var client = _context.Clients.Find(id);
            var stock = client.ActiveStocks + client.NotactiveStocks;
            return Json(new { total = stock });
        }
        public JsonResult GetClients()
        {
            List<Client> listItem = _context.Clients.Where(i =>  i.ActiveStocks > 0).ToList();

            return Json(listItem);
        }
    }
}
