using Ahlia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ahlia.Controllers
{
    public class BannedClientsApi : Controller
    {
        private readonly AhliahContext _context;

        public BannedClientsApi(AhliahContext context)
        {
            _context = context;
        }


        //https://localhost:7204/BannedClientsApi/AllBannedClients
        public async Task<IActionResult> AllBannedClients()
        {
            var ahliahContext = _context.BannedClients.Include(b => b.BannedType).Include(b => b.Client);
            return Json(await ahliahContext.ToListAsync());
        }

        //https://localhost:7204/BannedClientsApi/AllClientSelect
        public async Task<IActionResult> AllClientSelect()
        {
            var ahliahContext = _context.Clients.Where(c=>c.ClientStatus!= "ممنوع").Select(s => new
            {
                Id = s.Id,
                Name = s.FirstName
            });
            return Json( ahliahContext);
        }


        //https://localhost:7204/BannedClientsApi/BannedTypeSelect

        public async Task<IActionResult> BannedTypeSelect()
        {
            var ahliahContext = _context.Bannings.Select(s => new
            {
                Id = s.Id,
                type = s.BannedType
            });
            return Json(ahliahContext);
        }


        [HttpPost]
        public IActionResult Create( int ClientId, int BannedTypeId, DateTime Startdate,DateTime Enddate ,string Reason, string OrderedBy,[FromForm] IFormFile Photo)
        {

            BannedClient bannedClient = new BannedClient();
          //  if (ModelState.IsValid)
           // {
                var client = _context.Clients.Find(ClientId);
                client.ClientStatus = "ممنوع";
                _context.Update(client);

               if(Photo != null)
                {



                    MemoryStream ms = new MemoryStream();
                    Photo.CopyTo(ms);
                     bannedClient.Photo = ms.ToArray(); 
                  

                    ms.Close();
                    ms.Dispose();



                }
                bannedClient.ClientId = ClientId;
                bannedClient.BannedTypeId = BannedTypeId;
                bannedClient.Startdate = Startdate;
                bannedClient.Enddate = Enddate;
                bannedClient.Reason = Reason;
                bannedClient.OrderedBy = OrderedBy;
                _context.Add(bannedClient);
                 _context.SaveChanges();
                return Json(bannedClient);
           // }
            //ViewData["BannedTypeId"] = new SelectList(_context.Bannings, "Id", "BannedType", bannedClient.BannedTypeId);
            //ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FirstName", bannedClient.ClientId);
            //return View(bannedClient);
          // return Json("obj not added");
        }


       // https://localhost:7204/BannedClientsApi/cancelBAnning?id=8
        public IActionResult cancelBAnning(int id)
        {
            var bannedClient = _context.BannedClients.Find(id);
            var clientId = bannedClient.ClientId;
            var client = _context.Clients.Where(c => c.Id == clientId).FirstOrDefault();
            client.ClientStatus = "Active";
            _context.Update(client);
            _context.Remove(bannedClient);
            _context.SaveChanges();
            return Json(client);
        }








    }
}
