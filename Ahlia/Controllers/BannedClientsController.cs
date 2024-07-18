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
    
    public class BannedClientsController : Controller
    {
        private readonly AhliahContext _context;

        public BannedClientsController(AhliahContext context)
        {
            _context = context;
        }
        
        // GET: BannedClients
        public async Task<IActionResult> Index()
        {
            var ahliahContext = _context.BannedClients.Include(b => b.BannedType).Include(b => b.Client).Where(c=>c.Client.ClientStatus== "ممنوع"&&c.Enddate==null);
            return View(await ahliahContext.ToListAsync());
        }

        // GET: BannedClients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null || _context.StoppedClients == null)
            {
                return NotFound();
            }

            var stockPrice = await _context.BannedClients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockPrice.CancelImage != null)
            {
                string imageBase64Data =
  Convert.ToBase64String(stockPrice.CancelImage);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                //ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl = imageDataURL;
            }

            if (stockPrice == null)
            {
                return NotFound();
            }

            return View(stockPrice);
        }

        // GET: BannedClients/Create
        public IActionResult Create()
        {
            ViewData["BannedTypeId"] = new SelectList(_context.Bannings, "Id", "BannedType");
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(c => c.ClientStatus != "ممنوع"), "Id", "FirstName");
            return View();
        }

        // POST: BannedClients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,BannedTypeId,IsActive,Reason,Photo,OrderedBy")] BannedClient bannedClient)
        {
            if (ModelState.IsValid)
            {
                //var client = _context.Clients.Where(i => i.Id == bannedClient.ClientId);
                var client = _context.Clients.Find(bannedClient.ClientId);
                client.ClientStatus = "ممنوع";
                _context.Update(client);

                foreach (var file in Request.Form.Files)
                {



                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    if (bannedClient.Photo == null)
                    { bannedClient.Photo=ms.ToArray(); }
                    else
                    {
                        bannedClient.Photo =  ms.ToArray();
                    }

                    ms.Close();
                    ms.Dispose();



                }
                bannedClient.Startdate = System.DateTime.Now;
                _context.Add(bannedClient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BannedTypeId"] = new SelectList(_context.Bannings, "Id", "BannedType", bannedClient.BannedTypeId);
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(c => c.ClientStatus != "ممنوع"), "Id", "FirstName", bannedClient.ClientId);
            return View(bannedClient);
        }

        // GET: BannedClients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BannedClients == null)
            {
                return NotFound();
            }

            var bannedClient = await _context.BannedClients.FindAsync(id);
            if (bannedClient == null)
            {
                return NotFound();
            }
            ViewData["BannedTypeId"] = new SelectList(_context.Bannings, "Id", "BannedType", bannedClient.BannedTypeId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FirstName", bannedClient.ClientId);
            return View(bannedClient);
        }

        // POST: BannedClients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,BannedTypeId,Startdate,IsActive,Reason,OrderedBy,CancelImage")] BannedClient bannedClient, IFormFile? CancelImage)
        {
            if (id != bannedClient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (CancelImage != null)
                    {



                        MemoryStream ms = new MemoryStream();
                        CancelImage.CopyTo(ms);
                        bannedClient.CancelImage = ms.ToArray();

                        ms.Close();
                        ms.Dispose();
                        var client = _context.Clients.Where(c => c.Id == bannedClient.ClientId).FirstOrDefault();

                        client.ClientStatus = "Active";
                        _context.Update(client);

                        bannedClient.Enddate = DateTime.Now;
                        var existing=_context.BannedClients.Where(c => c.Id == bannedClient.Id).First();
                        existing.Enddate = bannedClient.Enddate;
                        existing.CancelImage = bannedClient.CancelImage;

                        _context.Update(existing);

                        _context.SaveChanges();


                    }
                    else
                    {
                        var client = _context.Clients.Where(c => c.Id == bannedClient.ClientId).FirstOrDefault();

                        client.ClientStatus = "Active";
                        _context.Update(client);

                        bannedClient.Enddate = DateTime.Now;


                        _context.Update(bannedClient);

                        _context.SaveChanges();
                    }




                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannedClientExists(bannedClient.Id))
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
            ViewData["BannedTypeId"] = new SelectList(_context.Bannings, "Id", "BannedType", bannedClient.BannedTypeId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FirstName", bannedClient.ClientId);
            return View(bannedClient);
        }

        // GET: BannedClients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BannedClients == null)
            {
                return NotFound();
            }

            var bannedClient = await _context.BannedClients
                .Include(b => b.BannedType)
                .Include(b => b.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bannedClient == null)
            {
                return NotFound();
            }

            return View(bannedClient);
        }

        // POST: BannedClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BannedClients == null)
            {
                return Problem("Entity set 'AhliahContext.BannedClients'  is null.");
            }
            var bannedClient = await _context.BannedClients.FindAsync(id);
            if (bannedClient != null)
            {
                _context.BannedClients.Remove(bannedClient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannedClientExists(int id)
        {
          return _context.BannedClients.Any(e => e.Id == id);
        }
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
