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
    public class ClientStokcksMovementsApi : Controller
    {
        private readonly AhliahContext _context;

        public ClientStokcksMovementsApi(AhliahContext context)
        {
            _context = context;
        }


        // https://localhost:7204/ClientStokcksMovementsApi/AllClientStokcksMovements
        public IActionResult AllClientStokcksMovements()
        {
            var ahliahContext = _context.ClientStokcksMovements.Include(c => c.Client).Include(c => c.MovementType).Include(c => c.NewClient).Select(s => new
            {
                Id = s.Id,
                newClient = s.NewClient.FirstName,
                oldClient=s.Client.FirstName,
                movemwntType=s.MovementType.MovementType,
                date=s.MovementDate,
                stockAmount=s.Amount,
                reason=s.Reason,
                note=s.Notes
            });
            return Json(ahliahContext.ToList());
        }

        // https://localhost:7204/ClientStokcksMovementsApi/Details?id=14
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

            return Json(clientStokcksMovement);
        }
        //https://localhost:7204/ClientStokcksMovementsApi/MovementTypeSelect
        public async Task<IActionResult> MovementTypeSelect()
        {
            var ahliahContext = _context.StocksMovements.Select(s => new
            {
                Id = s.Id,
                type = s.MovementType
            });
            return Json(ahliahContext);
        }
        //https://localhost:7204/ClientStokcksMovementsApi/OldClientSelect
        public async Task<IActionResult> OldClientSelect()
        {
            var ahliahContext = _context.Clients.Where(i => i.ClientStatus == "Active" && i.ActiveStocks > 0).Select(s => new
            {
                Id = s.Id,
                name = s.FirstName
            });
            return Json(ahliahContext);
        }
        //https://localhost:7204/ClientStokcksMovementsApi/NewClientSelect
        public async Task<IActionResult> NewClientSelect()
        {
            var ahliahContext = _context.Clients.Where(i => i.ClientStatus == "Active").Select(s => new
            {
                Id = s.Id,
                name = s.FirstName
            });
            return Json(ahliahContext);
        }



        //https://localhost:7204/ClientStokcksMovementsApi/Create?ClientId=74716&MovementTypeId=1&Amount=90&NewClientId=74715&MovementDate=15/04/2023&Reason=...&Notes=....

        [HttpPost]
        public IActionResult Create(int ClientId,int MovementTypeId,int Amount,int NewClientId,DateTime MovementDate,string Reason, string Notes)
        {
            
                ClientStokcksMovement clientStokcksMovement = new ClientStokcksMovement();
                clientStokcksMovement.ClientId = ClientId;
                clientStokcksMovement.MovementTypeId = MovementTypeId;
                clientStokcksMovement.Amount = Amount;
                clientStokcksMovement.NewClientId = NewClientId;
                clientStokcksMovement.MovementDate = MovementDate;
                clientStokcksMovement.Reason = Reason;
                clientStokcksMovement.Notes = Notes;
               

                var oldclient = _context.Clients.Find(clientStokcksMovement.ClientId);
                var newclient = _context.Clients.Find(clientStokcksMovement.NewClientId);
              
             
         
                oldclient.ActiveStocks = oldclient.ActiveStocks - clientStokcksMovement.Amount;
                    newclient.ActiveStocks = newclient.ActiveStocks + clientStokcksMovement.Amount;

                if (oldclient.ActiveStocks == 0)
               
                    oldclient.IsActive = false;


                    
               

                _context.Update(oldclient);

            if (oldclient.ActiveStocks < 0)
            {
                return Json("OldClientStocks<0");
            }
                    if (newclient.ActiveStocks > 0 && newclient.IsActive == false)
                    {
                        newclient.IsActive = true;
                    }
                    _context.Update(newclient);

                    _context.Add(clientStokcksMovement);

                     _context.SaveChanges();
                
                if (oldclient.ActiveStocks != 0)
                {
                    
                     string mssg = "أصبح عدد الأسهم لدى " + " "+oldclient.FirstName + oldclient.ActiveStocks + "  " + " وأصبح عدد الأسهم لدى" + "  "+newclient.FirstName +"  "+ newclient.ActiveStocks;
                   
                    return Json(mssg);
                }

                else
                return Json("أصبح عدد الأسهم لدى " + " " + oldclient.FirstName + oldclient.ActiveStocks + "  " + " وأصبح عدد الأسهم لدى" + "  " + newclient.FirstName + "  " + newclient.ActiveStocks);
           
            //return BadRequest();
        }

      
        // GET: ClientStokcksMovements/Delete/5

        // POST: ClientStokcksMovements/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClientStokcksMovements == null)
            {
                return Problem("Entity set 'AhliahContext.ClientStokcksMovements'  is null.");
            }
            var clientStokcksMovement = await _context.ClientStokcksMovements.FindAsync(id);
            if (clientStokcksMovement != null)
            {
                _context.ClientStokcksMovements.Remove(clientStokcksMovement);
            }
            
             _context.SaveChanges();
            return Json("تم الحذف");
        }

        private bool ClientStokcksMovementExists(int id)
        {
          return _context.ClientStokcksMovements.Any(e => e.Id == id);
        }
    }
}
