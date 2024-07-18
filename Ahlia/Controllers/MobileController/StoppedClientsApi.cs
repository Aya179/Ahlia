using Ahlia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ahlia.Controllers
{
    public class StoppedClientsApi : Controller
    {



        private readonly AhliahContext _context;

        public StoppedClientsApi(AhliahContext context)
        {
            _context = context;
        }
      //  https://localhost:7204/StoppedClientsApi/AllStoppedClients
        public IActionResult AllStoppedClients()
        {
            var ahliahContext = _context.StoppedClients.Include(b => b.StoppedType).Include(b => b.Client);
            return Json( ahliahContext.ToList());
        }
        //  https://localhost:7204/StoppedClientsApi/AllClientSelect

        public async Task<IActionResult> AllClientSelect()
        {
            var ahliahContext = _context.Clients.Where(c => c.ClientStatus != "غيرمودع").Select(s => new
            {
                Id = s.Id,
                Name = s.FirstName
            });
            return Json(ahliahContext);
        }
        //https://localhost:7204/StoppedClientsApi/StoppedTypeSelect
        public async Task<IActionResult> StoppedTypeSelect()
        {
            var ahliahContext = _context.Stoppings.Select(s => new
            {
                Id = s.Id,
                type = s.StoppedStatus
            });
            return Json(ahliahContext);
        }


       // https://localhost:7204/StoppedClientsApi/Create?ClientId=74714&StoppedTypeId=1&StartDate=16/04/2023&Enddate=16/05/2023&Reason=.....        [HttpPost]

        public IActionResult Create(int ClientId,int StoppedTypeId,DateTime StartDate,DateTime Enddate,string Reason )
        {

            StoppedClient stoppedClient = new StoppedClient();

                //var client = _context.Clients.Where(i => i.Id == stoppedClient.ClientId).FirstOrDefaultAsync();
                var client = _context.Clients.Find(ClientId);
                client.ClientStatus = "غيرمودع";
                _context.Update(client);
            stoppedClient.ClientId = ClientId;
            stoppedClient.StoppedTypeId = StoppedTypeId;
            stoppedClient.StartDate = StartDate;
            stoppedClient.Enddate= Enddate;
            stoppedClient.Reason = Reason;

                _context.Add(stoppedClient);
                _context.SaveChanges();
            return Json(stoppedClient);
           
        }
        //https://localhost:7204/StoppedClientsApi/cancelStoppe?id=4
        [HttpPost]
        public IActionResult cancelStoppe(int id)
        {
            var StoppedClient = _context.StoppedClients.Find(id);
            var clientId = StoppedClient.ClientId;
            var client = _context.Clients.Where(c => c.Id == clientId).FirstOrDefault();
            client.ClientStatus = "Active";
            _context.Update(client);
            _context.Remove(StoppedClient);
            _context.SaveChanges();
            return Json(client);
        }

    }
}
