using Ahlia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace Ahlia.Controllers
{
    public class StoppedClientController : Controller
    {
        private readonly AhliahContext _context;
        public StoppedClientController(AhliahContext context)
        {
            _context = context;
        }
        

        public async Task<IActionResult> Index()
        {
            var ahliahContext = _context.StoppedClients.Include(b => b.StoppedType).Include(b => b.Client).Where(b=>b.Client.ClientStatus== "غيرمودع"&&b.Enddate==null);
            return View(await ahliahContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["StoppedStatusId"] = new SelectList(_context.Stoppings, "Id", "StoppedStatus");
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(c => c.ClientStatus == "Active"), "Id", "FirstName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("ClientId,StoppedTypeId,StartDate,Enddate,Reason,Photo")] StoppedClient stoppedClient,IFormFile ? photo)
        {
            if (ModelState.IsValid)
            {


                //var client = _context.Clients.Where(i => i.Id == stoppedClient.ClientId).FirstOrDefaultAsync();
                var client = _context.Clients.Find(stoppedClient.ClientId);
                client.ClientStatus = "غيرمودع";
                _context.Update(client);

                stoppedClient.StartDate = DateTime.Now;


                if (photo != null)
                {

                    MemoryStream ms = new MemoryStream();
                    photo.CopyTo(ms);
                    stoppedClient.Photo = ms.ToArray();


                    ms.Close();
                    ms.Dispose();
                }


                _context.Add(stoppedClient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoppedStatusId"] = new SelectList(_context.Stoppings, "Id", "StoppedStatus", stoppedClient.StoppedTypeId);

            ViewData["ClientId"] = new SelectList(_context.Clients.Where(c => c.ClientStatus == "Active" || c.ClientStatus == "ممنوع"), "Id", "FirstName", stoppedClient.ClientId);
            return View(stoppedClient);
        }
        
        


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StoppedClients == null)
            {
                return NotFound();
            }

            var stockPrice = await _context.StoppedClients.Include(b => b.StoppedType).Include(b => b.Client)
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StoppedClients == null)
            {
                return NotFound();
            }

            var stockPrice = await _context.StoppedClients.FindAsync(id);
            if (stockPrice == null)
            {
                return NotFound();
            }
            return View(stockPrice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CancelImage,Reason,Enddate,StartDate,StoppedTypeId,ClientId")] StoppedClient stoppedClient, IFormFile? CancelImage)
        {
            if (id != stoppedClient.Id)
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
                        stoppedClient.CancelImage = ms.ToArray();

                        ms.Close();
                        ms.Dispose();




                        var client = _context.Clients.Where(c => c.Id == stoppedClient.ClientId).FirstOrDefault();
                        client.ClientStatus = "Active";
                        stoppedClient.Enddate = DateTime.Now;
                        _context.Update(client);
                        var exist = _context.StoppedClients.Find(id);
                        exist.ClientId = stoppedClient.ClientId;
                        exist.CancelImage = stoppedClient.CancelImage;
                        exist.Enddate = stoppedClient.Enddate;

                        _context.Update(exist);

                        _context.SaveChanges();

                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!stoppedclientExists(stoppedClient.Id))
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
            return View(stoppedClient);
        }
        private bool stoppedclientExists(int id)
        {
            return _context.StoppedClients.Any(e => e.Id == id);
        }


        public async Task<IActionResult> EditToActive(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", client.CityId);
            ViewData["ClientTypeId"] = new SelectList(_context.ClientTypes, "TypeId", "TypeName", client.ClientTypeId);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditToActive(int id, [Bind("Id,FirstName,MiddleName,LastName,Mother,NationalId,NationalIdType,Mobile1,Mobile2,AddressDomicil,AddressWork,OriginalAddress,Nationality,ClientStatus,IsActive,HomePhone,Fax,Khana,Birthdate,Birthcity,ClientTypeId,BankAccount,Idphoto,IdcardPhoto,Notes,IsAlive,IsDeleted,CityId,ActiveStocks,NotactiveStocks")] Client client, IFormFile Idphoto, IFormFile IdcardPhoto)
        {
            if (id != client.Id)
            {
                return NotFound();
            }


          

           
                var existingClient = _context.Clients.Find(id);
                existingClient.Mobile1 = client.Mobile1;
                existingClient.Mobile2 = client.Mobile2;
                existingClient.IsDeleted = client.IsDeleted;
                existingClient.Notes = client.Notes;
                existingClient.AddressWork = client.AddressWork;
                existingClient.ActiveStocks = client.ActiveStocks;
                existingClient.AddressDomicil = client.AddressDomicil;
                existingClient.HomePhone = client.HomePhone;

                existingClient.Birthdate = client.Birthdate;
                existingClient.BankAccount = client.BankAccount;
                existingClient.Birthcity = client.Birthcity;
                existingClient.CityId = client.CityId;
                existingClient.NationalId = client.NationalId;
                existingClient.Nationality = client.Nationality;
                existingClient.NationalIdType = client.NationalIdType;
                existingClient.ClientStatus = client.ClientStatus;
                existingClient.ClientTypeId = client.ClientTypeId;
                existingClient.Fax = client.Fax;
                existingClient.FirstName = client.FirstName;
                existingClient.LastName = client.LastName;
                existingClient.IsActive = client.IsActive;
                existingClient.IsAlive = client.IsAlive;
                existingClient.Khana = client.Khana;
                existingClient.MiddleName = client.MiddleName;
                existingClient.Mother = client.Mother;
                existingClient.OriginalAddress = client.OriginalAddress;
                existingClient.NotactiveStocks = client.NotactiveStocks;
                // if (existingClient.FirstName != null && existingClient.LastName != null && existingClient.MiddleName != null && existingClient.BankAccount != null && existingClient.HomePhone != null && existingClient.OriginalAddress != null && existingClient.NationalId != null && existingClient.ActiveStocks != null && existingClient.NotactiveStocks != null)
                if (existingClient.FirstName != null && existingClient.LastName != null && existingClient.MiddleName != null && existingClient.Mother != null && existingClient.HomePhone != null || existingClient.OriginalAddress != null && existingClient.Mobile1 != null && existingClient.Mobile2 != null && existingClient.Nationality != null && existingClient.Fax != null && existingClient.CityId != null && existingClient.ActiveStocks != null && existingClient.NotactiveStocks != null && existingClient.NationalId != null )

                {
                    existingClient.ClientStatus = "Active";






                    var clientstopped = _context.StoppedClients.Where(c => c.ClientId == client.Id).First();
                    if (clientstopped != null)
                    {
                        clientstopped.Enddate = DateTime.Now;
                        _context.Update(clientstopped);
                        _context.SaveChanges();
                    }


                }



                _context.Update(existingClient);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            

        }



    }
}
