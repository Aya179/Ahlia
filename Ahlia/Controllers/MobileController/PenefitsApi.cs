using Ahlia.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ahlia.Controllers
{
    public class PenefitsApi : Controller
    {



        private readonly AhliahContext _context;

        public PenefitsApi(AhliahContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> AllClientspenfits()
        {


            var sub = from od in _context.Penefits
                      join q in _context.Clients on od.ClientId equals q.Id
                      //join price in _context.pr on od.ClientId equals q.Id
                      // where q.CustomerId == customerId
                      group new { od } by new { od.ClientId, }
                          into v
                      select new
                      {
                          ClientId = v.Key.ClientId,


                          TotalPenefits = v.Sum(x => x.od.CompleteAmount)

                      };
            var y = sub.ToList();
            var query = from client in _context.Clients
                        join vs in sub on client.Id equals vs.ClientId
                        //join branch in _context.Branches on vs.bId equals branch.BranchId
                        // join room in _context.QutationRoom on id equals room.id



                        select new
                        {
                            clientId = client.Id,
                            totalPenefits = vs.TotalPenefits,
                            clientName = client.FirstName,
                        }
                        ;
            var x = query.ToList();






            return Json(query);

        }


        //https://localhost:7204/PenefitsApi/detailsClientPenefitsPerYear?clientId=74713
        public async Task<IActionResult> detailsClientPenefitsPerYear(int clientId)
        {
            var sub = from od in _context.Penefits
                      join q in _context.Clients on od.ClientId equals q.Id
                      join p in _context.StockPrices on od.PriceId equals p.Id

                      //join price in _context.pr on od.ClientId equals q.Id
                      where q.Id == clientId
                      group new { od, p } by new { od.ClientId, p.Year }
                          into v
                      select new
                      {
                          ClientId = v.Key.ClientId,

                          year = v.Key.Year,
                          TotalPenefits = v.Sum(x => x.od.CompleteAmount)

                      };
            var y = sub.ToList();
            var query = from client in _context.Clients
                        join vs in sub on client.Id equals vs.ClientId
                        //join branch in _context.Branches on vs.bId equals branch.BranchId
                        // join room in _context.QutationRoom on id equals room.id



                        select new
                        {
                            year = vs.year,
                            total = vs.TotalPenefits,
                            clientName = client.FirstName,
                        }
                        ;
            var x = query.ToList();






            return Json(query);

        }

        //https://localhost:7204/PenefitsApi/yearspenfits
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
                          year = v.Key.Year,


                          TotalYearPenefits = v.Sum(x => x.od.CompleteAmount)

                      };
            var y = sub.ToList();






            return Json(y);

        }


    }
}
