using Ahlia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace Ahlia.Controllers.MobileController
{
    public class ClientApi : Controller
    {
        private readonly AhliahContext _context;

        public ClientApi(AhliahContext context)
        {
            _context = context;
        }
        public IActionResult GetAllClients()
        {
            var client = _context.Clients.ToList();
            return Json(client);
        }
        //chang bannedClient to stopped
        public IActionResult AddClient(string FirstName, string lastName, string MiddleName, string Mother, string NationalId, string NationalIdType, string Mobile1, string Mobile2, string AddressDomicil, string AddressWork, string OriginalAddress, string Nationality, string HomePhone, string Fax, string Khana, DateTime Birthdate, string BankAccount, IFormFile Idphoto, IFormFile IdcardPhoto, string Notes, int ActiveStocks, int NotactiveStocks, string Birthcity)
        {
            Client client = new Client();
            client.FirstName = FirstName;
            client.MiddleName = MiddleName;
            client.LastName = lastName;
            client.Mother = Mother;
            client.Mobile1 = Mobile1;
            client.Mobile2 = Mobile2;
            client.AddressWork = AddressWork;
            client.AddressDomicil = AddressDomicil;
            client.HomePhone = HomePhone;
            client.Birthdate = Birthdate;
            client.ActiveStocks = ActiveStocks;
            client.NotactiveStocks = NotactiveStocks;
            client.OriginalAddress = OriginalAddress;
            client.Notes = Notes;

            client.Fax = Fax;
            client.Khana = Khana;
            client.Birthcity = Birthcity;
            client.BankAccount = BankAccount;
            client.Nationality = Nationality; ;
            client.NationalId = NationalId;
            client.NationalIdType = NationalIdType;

            if (IdcardPhoto != null && Idphoto != null)
            {
                foreach (var file in Request.Form.Files)
                {



                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    if (client.Idphoto == null)
                    { client.Idphoto = ms.ToArray(); }
                    else
                    {
                        client.IdcardPhoto = ms.ToArray();
                    }

                    ms.Close();
                    ms.Dispose();



                }
                if (client.FirstName == null || client.LastName == null || client.MiddleName == null || client.BankAccount == null || client.HomePhone == null || client.OriginalAddress == null || client.NationalId == null)
                {
                    client.ClientStatus = "ممنوع";

                    client.IsActive = true;
                    _context.Add(client);
                    _context.SaveChanges();



                    var clientbanned = new BannedClient();
                    clientbanned.ClientId = client.Id;
                    _context.Add(clientbanned);
                    _context.SaveChanges();
                    Penefit clientPenefit = new Penefit();
                    clientPenefit.ClientId = client.Id;
                    clientPenefit.CompleteAmount = 0;
                    _context.Add(clientPenefit);
                    _context.SaveChanges();
                    Payment clientPayment = new Payment();
                    clientPayment.ClientId = client.Id;
                    clientPayment.Amount = 0;
                    _context.Add(clientPayment);
                    _context.SaveChanges();
                    return Json(client);
                }
                else
                {
                    client.IsActive = true;
                    client.ClientStatus = "Active";


                    _context.Add(client);
                    _context.SaveChanges();
                    Penefit clientPenefit = new Penefit();
                    clientPenefit.ClientId = client.Id;
                    clientPenefit.CompleteAmount = 0;
                    _context.Add(clientPenefit);
                    _context.SaveChanges();
                    Payment clientPayment = new Payment();
                    clientPayment.ClientId = client.Id;
                    clientPayment.Amount = 0;
                    _context.Add(clientPayment);
                    _context.SaveChanges();
                    return Json(client);
                }
            }
            if (IdcardPhoto != null && Idphoto == null)
            {
                foreach (var file in Request.Form.Files)
                {



                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);


                    client.IdcardPhoto = ms.ToArray();


                    ms.Close();
                    ms.Dispose();



                }
                if (client.FirstName == null || client.LastName == null || client.MiddleName == null || client.BankAccount == null || client.HomePhone == null || client.OriginalAddress == null || client.NationalId == null)
                {
                    client.ClientStatus = "ممنوع";

                    client.IsActive = true;
                    _context.Add(client);
                    _context.SaveChanges();



                    var clientbanned = new BannedClient();
                    clientbanned.ClientId = client.Id;
                    _context.Add(clientbanned);
                    _context.SaveChanges();
                    Penefit clientPenefit = new Penefit();
                    clientPenefit.ClientId = client.Id;
                    clientPenefit.CompleteAmount = 0;
                    _context.Add(clientPenefit);
                    _context.SaveChanges();
                    Payment clientPayment = new Payment();
                    clientPayment.ClientId = client.Id;
                    clientPayment.Amount = 0;
                    _context.Add(clientPayment);
                    _context.SaveChanges();
                    return Json(client);
                }
                else
                {
                    client.IsActive = true;
                    client.ClientStatus = "Active";


                    _context.Add(client);
                    _context.SaveChanges();
                    Penefit clientPenefit = new Penefit();
                    clientPenefit.ClientId = client.Id;
                    clientPenefit.CompleteAmount = 0;
                    _context.Add(clientPenefit);
                    _context.SaveChanges();
                    Payment clientPayment = new Payment();
                    clientPayment.ClientId = client.Id;
                    clientPayment.Amount = 0;
                    _context.Add(clientPayment);
                    _context.SaveChanges();
                    return Json(client);
                }
            }
            if (IdcardPhoto == null && Idphoto != null)
            {
                foreach (var file in Request.Form.Files)
                {



                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);


                    client.Idphoto = ms.ToArray();


                    ms.Close();
                    ms.Dispose();



                }

                client.ClientStatus = "ممنوع";

                client.IsActive = true;
                _context.Add(client);
                _context.SaveChanges();



                var clientbanned = new BannedClient();
                clientbanned.ClientId = client.Id;
                _context.Add(clientbanned);
                _context.SaveChanges();
                Penefit clientPenefit = new Penefit();
                clientPenefit.ClientId = client.Id;
                clientPenefit.CompleteAmount = 0;
                _context.Add(clientPenefit);
                _context.SaveChanges();
                Payment clientPayment = new Payment();
                clientPayment.ClientId = client.Id;
                clientPayment.Amount = 0;
                _context.Add(clientPayment);
                _context.SaveChanges();
                return Json(client);


            }

            else
            {

                client.ClientStatus = "ممنوع";

                client.IsActive = true;
                _context.Add(client);
                _context.SaveChanges();



                var clientbanned = new BannedClient();
                clientbanned.ClientId = client.Id;
                _context.Add(clientbanned);
                _context.SaveChanges();
                Penefit clientPenefit = new Penefit();
                clientPenefit.ClientId = client.Id;
                clientPenefit.CompleteAmount = 0;
                _context.Add(clientPenefit);
                _context.SaveChanges();
                Payment clientPayment = new Payment();
                clientPayment.ClientId = client.Id;
                clientPayment.Amount = 0;
                _context.Add(clientPayment);
                _context.SaveChanges();
                return Json(client);

            }
        }
        public IActionResult DeleteClient(int id)
        {
            var client = _context.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            else
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
                return Json("تم الحذف بنجاح");
            }
        }

        public IActionResult GetAllCities()
        {
            var cities = _context.Cities.Select(c => new
            {
                id = c.CityId,
                name=c.CityName
            }
            ) ;
            return Json(cities);
        }

        public IActionResult GetAllClientType()
        {
            var clientType = _context.ClientTypes.Select(c => new 
            {
                id=c.TypeId,
                name=c.TypeName
            });
            return Json(clientType);
        }

        public IActionResult ClientPayment(int clientId)
        {

            var clientPayment = _context.Payments
                .Where(p => p.ClientId == clientId && p.Amount > 0).Include(p => p.Client);

            var sub = from od in _context.Penefits
                      join q in _context.Clients on od.ClientId equals q.Id
                      join p in _context.Payments on q.Id equals p.ClientId

                      where q.Id == clientId
                      group new { od, p } by new { od.ClientId, p.Client.Id }
                          into v
                      select new
                      {

                          PeniftsValue = v.Sum(x => x.od.CompleteAmount),
                          PaymentsValue = v.Sum(x => x.p.Amount),


                      };
            var y = sub;

            return Json(sub.ToList());

        }

        public IActionResult ClientPenefit(int clientId)
        {
            var ahliahContext = _context.Penefits.Where(p => p.ClientId == clientId && p.CompleteAmount > 0);
            return Json(ahliahContext.ToList());
        }
        public IActionResult ClientDetails(int id)
        {
            var client = _context.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            else
            {

                return Json(client);
            }
        }

        public IActionResult UpdateClient(int Id, string FirstName, string MiddleName, string LastName, string Mother, string NationalId, string NationalIdType, string Mobile1, string Mobile2, string AddressDomicil, string AddressWork, string OriginalAddress, string Nationality, string HomePhone, string Fax, string Khana, DateTime Birthdate, string Birthcity, string BankAccount, string Notes, int ActiveStocks, int NotactiveStocks)
        {
            var client = _context.Clients.Where(c => c.Id == Id).FirstOrDefault();
            if (client == null)
            {
                return NotFound();
            }
            else
            {
                if (FirstName != null)
                {
                    client.FirstName = FirstName;
                }
                if (LastName != null)
                {
                    client.LastName = FirstName;
                }
                if (MiddleName != null)
                {
                    client.MiddleName = FirstName;
                }
                if (Mother != null)
                {
                    client.Mother = FirstName;
                }
                if (NationalId != null)
                {
                    client.NationalId = FirstName;
                }
                if (NationalIdType != null)
                {
                    client.NationalIdType = FirstName;
                }
                if (Nationality != null)
                {
                    client.Nationality = FirstName;
                }
                if (AddressDomicil != null)
                {
                    client.AddressDomicil = AddressDomicil;
                }
                if (AddressWork != null)
                {
                    client.AddressWork = FirstName;
                }
                if (ActiveStocks != null)
                {
                    client.ActiveStocks = ActiveStocks;
                }
                if (NotactiveStocks != null)
                {
                    client.NotactiveStocks = NotactiveStocks;
                }
                if (BankAccount != null)
                {
                    client.BankAccount = BankAccount;
                }
                if (Khana != null)
                {
                    client.Khana = Khana;
                }
                if (Mobile1 != null)
                {
                    client.Mobile1 = Mobile1;
                }
                if (Mobile2 != null)
                {
                    client.Mobile2 = Mobile2;
                }
                if (OriginalAddress != null)
                {
                    client.OriginalAddress = FirstName;
                }
                if(Fax!=null)
                {
                    client.Fax = Fax;
                }
                if(HomePhone!=null)
                {
                    client.HomePhone = HomePhone;
                }
                if(Khana!=null)
                {
                    client.Khana = Khana;
                }
                if(Birthcity!=null)
                {
                    client.Birthcity = Birthcity;
                }
                if(Birthdate!=null)
                {
                    client.Birthcity=Birthcity;
                }
                if(Notes!=null)
                {
                    client.Notes = Notes;
                }
                _context.Update(client);
                _context.SaveChanges();
                return Json(client);
                
            

            }


        }
        public IActionResult UpdateClienImg(int id, [FromForm] IFormFile Idphoto, [FromForm] IFormFile IdcardPhoto)
        {
            var client = _context.Clients.Find(id);



            if (IdcardPhoto!=null)
            {
                MemoryStream ms = new MemoryStream();
                IdcardPhoto.CopyTo(ms);
                client.IdcardPhoto = ms.ToArray();

                ms.Close();
                ms.Dispose();

            }
            if (Idphoto != null)
            {
                MemoryStream ms = new MemoryStream();
                Idphoto.CopyTo(ms);
                client.Idphoto = ms.ToArray();

                ms.Close();
                ms.Dispose();

            }
            _context.Update(client);
            _context.SaveChanges();

            return Json(client);
        }
    }
}
