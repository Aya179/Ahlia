using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ahlia.Models;
using System.Data;
using ExcelDataReader;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using MimeKit;

namespace Ahlia.Controllers
{
    
    public class ClientsController : Controller
    {
        private readonly AhliahContext _context;
        IConfiguration configuration;
        IWebHostEnvironment hostEnvironment;
        IExcelDataReader reader;

        public ClientsController(AhliahContext context, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.configuration = configuration;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: Clients

       

        public async Task<IActionResult> ClientPaymentIndex(int clientId)
        {
            ViewBag.ClientId = clientId;

            var ahliahContext = _context.Payments.Include(p => p.CheckerNavigation).Include(p => p.City).Include(p => p.Client).Include(p => p.EditorNavigation).Where(p=>p.ClientId==clientId);
            return View(await ahliahContext.ToListAsync());
        }


        public async Task<IActionResult> ClientPenefit(int clientId)
        {
            ViewBag.NewClientId = clientId;

            var ahliahContext = _context.Penefits.Include(p => p.Client).Include(p => p.Price).Where(p => p.ClientId == clientId &&p.CompleteAmount>0);
            return View(await ahliahContext.ToListAsync());
        }
        public async Task<IActionResult> CreateClientPayment(int clientId)
        {
            ViewBag.NewClientId = clientId;

            //var ahliahContext = _context.Penefits.Include(p => p.Client).Include(p => p.Price).Where(p => p.ClientId == clientId && p.CompleteAmount > 0);
            return RedirectToAction("Create", "Payments");
        }



        //public IActionResult CreateClientPayment(int clientId)
        //{
        //    ViewData["Checker"] = new SelectList(_context.Employees, "Id", "EmployeeName");
        //    ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
        //   var client=_context.Clients.Fi(rstOrDefault(c=>c.Id==clientId);   
        //    ViewBag.ClientId = clientId;
        //    ViewBag.Clientname = client.FirstName;
        //    ViewData["Editor"] = new SelectList(_context.Employees, "Id", "EmployeeName");
        //    return View();
        //}

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClientPayment2([Bind("Id,Paymentdate,Amount,ClientId,CityId,BranchName,PayementFor,Editor,Checker,CheckNumber,BankAccount,CheckDate,ReceiverName,ReceiverNumber,RecieveDate,ReceiverResidance,Notes,IsDeleted,IsPayed,CardNumber")] Payment payment)
        {
            if (ModelState.IsValid)
            {
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
                        _context.Add(payment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("ClientPaymentIndex", "Clients", new { clientId = payment.ClientId });
                    }
                    else
                    {
                        ViewBag.msg = "firstelse";
                        ViewData["Checker"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Checker);
                        ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", payment.CityId);
                        ViewData["Editor"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Editor);
                        var client1 = _context.Clients.FirstOrDefault(c => c.Id == payment.ClientId);
                        ViewBag.ClientId = client1.Id;
                        ViewBag.Clientname = client1.FirstName;
                        return View(payment);
                    }
                }

                else
                {
                    ViewBag.msg = "secondelse";
                    ViewData["Checker"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Checker);
                    ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", payment.CityId);
                    ViewData["Editor"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Editor);
                    var client2 = _context.Clients.FirstOrDefault(c => c.Id == payment.ClientId);
                    ViewBag.ClientId = client2.Id;
                    ViewBag.Clientname = client2.FirstName;
                    return View(payment);
                }

            }
            ViewData["Checker"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Checker);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", payment.CityId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FirstName", payment.ClientId);
            ViewData["Editor"] = new SelectList(_context.Employees, "Id", "EmployeeName", payment.Editor);
            var client3 = _context.Clients.FirstOrDefault(c => c.Id == payment.ClientId);
            ViewBag.ClientId = client3.Id;
            ViewBag.Clientname = client3.FirstName;
            return View(payment);
        }


        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.City)
                .Include(c => c.ClientType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client.Idphoto != null)
            {
                string imageBase64Data =
  Convert.ToBase64String(client.Idphoto);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                //ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl = imageDataURL;
            }
            if (client.IdcardPhoto != null)
            {
                string imageBase64Data =
  Convert.ToBase64String(client.IdcardPhoto);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                //ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl1 = imageDataURL;
            }


            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            ViewData["ClientTypeId"] = new SelectList(_context.ClientTypes, "TypeId", "TypeName");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Mother,NationalId,NationalIdType,Mobile1,Mobile2,AddressDomicil,AddressWork,OriginalAddress,Nationality,IsActive,HomePhone,Fax,Khana,Birthdate,Birthcity,ClientTypeId,BankAccount,Idphoto,IdcardPhoto,Notes,IsAlive,IsDeleted,CityId,ActiveStocks,NotactiveStocks")] Client client, IFormFile Idphoto, IFormFile IdcardPhoto)
        {
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

               

            }

            //  if (client.FirstName == null || client.LastName == null || client.MiddleName == null || client.BankAccount == null || client.HomePhone == null || client.OriginalAddress == null || client.NationalId == null)
            if (_context.Clients.Any(x => x.NationalId == client.NationalId))
            {
                ViewBag.messge= "هذا الرقم الوطني موجود مسبقا";
                ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", client.CityId);
                ViewData["ClientTypeId"] = new SelectList(_context.ClientTypes, "TypeId", "TypeName", client.ClientTypeId);
                return View(client);
                 }
            else
            {
                if (client.FirstName == null || client.LastName == null || client.MiddleName == null || client.Mother == null || client.HomePhone == null || client.OriginalAddress == null || client.Mobile1 == null || client.Mobile2 == null || client.Nationality == null || client.Fax == null || client.CityId == null || client.ActiveStocks == null || client.NotactiveStocks == null || client.ClientTypeId == null && client.NationalId != null)
                {
                    client.ClientStatus = "غيرمودع";
                    if (client.ActiveStocks == 0 || client.ActiveStocks == null)
                        client.IsActive = false;
                    else client.IsActive = true;
                    string firstname = client.FirstName;
                    client.FirstName = firstname + " " + client.MiddleName + " " + client.LastName;
                    client.clientnumber = "-";
                    
                    //client.IsAlive = true;
                    _context.Add(client);
                    await _context.SaveChangesAsync();



                    var clientstopped = new StoppedClient();
                    clientstopped.ClientId = client.Id;
                    clientstopped.StartDate = DateTime.Now;
                    _context.Add(clientstopped);
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
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    client.IsActive = true;
                    client.ClientStatus = "Active";
                    string firstname = client.FirstName;
                    client.FirstName = firstname + " " + client.MiddleName + " " + client.LastName;
                    client.clientnumber= "-";
                    //لما بكون الFIRSTNAMEهو الاسم الأول
                    //client.IsAlive = true;

                    _context.Add(client);
                    await _context.SaveChangesAsync();
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
                    return RedirectToAction(nameof(Index));
                }
            }
        

            // }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", client.CityId);
            ViewData["ClientTypeId"] = new SelectList(_context.ClientTypes, "TypeId", "TypeName", client.ClientTypeId);
            return View(client);
        }


        public async Task<IActionResult> arraysFromExcelapi(IFormFile file1)
        {

            // Check the File is received
            //sheet1:مودع
            //sheet2:مودع غير
            List<Client> sheet1new = new List<Client>();
            List<Client> sheet1update = new List<Client>();
            List<Client> sheet1beforupdate = new List<Client>();
            List<Client> sheet2new = new List<Client>();
            List<Client> sheet2update = new List<Client>();
            List<Client> sheet2beforeupdate = new List<Client>();
            if (file1 == null)
                throw new Exception("File is Not Received...");

            int count = 0;
            // Create the Directory if it is not exist
            string dirPath = Path.Combine(hostEnvironment.WebRootPath, "ReceivedReports");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            
            // MAke sure that only Excel file is used 
            string dataFileName = Path.GetFileName(file1.FileName);

            string extension = Path.GetExtension(dataFileName);

            string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

            if (!allowedExtsnions.Contains(extension))
                throw new Exception("Sorry! This file is not allowed,  make sure that file having extension as either.xls or.xlsx is uploaded.");

            // Make a Copy of the Posted File from the Received HTTP Request
            string saveToPath = Path.Combine(dirPath, dataFileName);

            using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
            {
                file1.CopyTo(stream);
            }

            // USe this to handle Encodeing differences in .NET Core
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            // read the excel file
            using (var stream = new FileStream(saveToPath, FileMode.Open))
            {
                if (extension == ".xls")
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                else
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                //

                DataSet ds = new DataSet();
                ds = reader.AsDataSet();
                reader.Close();
               

                if (ds != null && ds.Tables.Count > 0)
                {
                    //Read the the Table sheet2 table[1] غيرمودع
                    // old:
                    // DataTable serviceDetails = ds.Tables[1];
                    DataTable serviceDetails = ds.Tables[0];

                    for (int i = 3430; i <= serviceDetails.Rows.Count; i++)//serviceDetails.Rows.Coun
                    {
                        Client details = new Client(); 
                       //string clientnmae = serviceDetails.Rows[i][15].ToString();
                        string name = serviceDetails.Rows[i][0].ToString();
                        var client = _context.Clients.Where(i => i.FirstName == name).FirstOrDefault();
                      
                        //if (client == null) 
                        //{
                        //    count++;
                        //    continue;
                        //}
                           
                        //client.clientnumber = clientnumber;
                        Payment payment= new Payment();
                        int clientid = client.Id;
                        int cityid = (int)client.CityId;
                        decimal amount = Convert.ToDecimal(serviceDetails.Rows[i][2]);
                        //var payment=_context.Payments.Where(p=>p.ClientId== clientid).FirstOrDefault();
                        //details.clientnumber = serviceDetails.Rows[i][14].ToString();
                        payment.Amount = amount;
                        payment.CityId = cityid;
                        payment.ClientId= clientid;
                        payment.Paymentdate = System.DateTime.UtcNow;
                        _context.Payments.Add(payment);
                        _context.SaveChanges();
                        //  {
                                                                                       //City c = new City();
                                                                                       //c.CityName = serviceDetails.Rows[i][2].ToString();
                                                                                       //_context.Add(c);
                                                                                       //_context.SaveChanges();
                                                                                       //details.CityId = c.CityId;
                                                                                       //details.CityId = 22;
                                                                                       // }
                                                                                       //else
                                                                                       //{
                                                                                       //    details.CityId = city.CityId;
                                                                                       //}

                        //var clientType = _context.ClientTypes.Where(c => c.TypeName == serviceDetails.Rows[i][6].ToString()).FirstOrDefault();
                        //if (clientType == null)
                        //{
                        //    ClientType ct = new ClientType();
                        //    ct.TypeName = serviceDetails.Rows[i][6].ToString();
                        //    _context.Add(ct);
                        //    _context.SaveChanges();
                        //    details.ClientTypeId = ct.TypeId;
                        //}
                        //else
                        //{
                        // details.ClientTypeId = clientType.TypeId;
                        //details.ClientTypeId = 11;
                        ////}
                        //details.Nationality = serviceDetails.Rows[i][7].ToString();//5;
                        //                                                           // details.NationalIdType = serviceDetails.Rows[i][7].ToString();
                        //details.NationalIdType = "هوية شخصية";
                        //details.NationalId = serviceDetails.Rows[i][5].ToString();//11
                        //details.NotactiveStocks = /*Convert.ToInt32(serviceDetails.Rows[i][9])*/0;
                        //details.ActiveStocks = Convert.ToInt32(serviceDetails.Rows[i][12]);//10
                        //details.Mother = serviceDetails.Rows[i][3].ToString();//13
                        //details.MiddleName = serviceDetails.Rows[i][2].ToString();//14
                        //details.FirstName = serviceDetails.Rows[i][0].ToString();//15
                        //details.LastName = serviceDetails.Rows[i][4].ToString();//12
                        //details.Mobile1 = serviceDetails.Rows[i][8].ToString();
                        //details.clientnumber = serviceDetails.Rows[i][14].ToString();//17
                        //details.Notes = serviceDetails.Rows[i][13].ToString();

                        //details.IsActive = true;


                        //details.ClientStatus = "غيرمودع";
                        //if (details.MiddleName == "")
                        //{
                        //    details.MiddleName = "**";
                        //}
                        //if (details.Mother == "")
                        //{
                        //    details.Mother = "**";
                        //}
                        //if (details.LastName == "")
                        //{
                        //    details.LastName = "**";
                        //}

                        //if (details.Fax == "")
                        //{
                        //    details.Fax = "**";
                        //}
                        //if (details.NationalId == "")
                        //{
                        //    details.NationalId = "**";
                        //}
                        //if (details.NationalIdType == "")
                        //{
                        //    details.NationalIdType = "**";
                        //}
                        //if (details.Mobile1 == "")
                        //{
                        //    details.Mobile1 = "**";
                        //}
                        //if (details.Mobile2 == "")
                        //{
                        //    details.Mobile2 = "**";
                        //}
                        //if (details.OriginalAddress == "")
                        //{
                        //    details.OriginalAddress = "**";
                        //}
                        //if (details.Nationality == "")
                        //{
                        //    details.Nationality = "**";
                        //}
                        //if (details.HomePhone == "")
                        //{
                        //    details.HomePhone = "**";
                        //}
                        //if (details.clientnumber == "")
                        //{
                        //    details.clientnumber = "**";
                        //}

                        //var checkClient = _context.Clients.Where(c => c.FirstName == serviceDetails.Rows[i][0].ToString()).FirstOrDefault();//15
                        //if (checkClient == null)
                        // {
                        // Add the record in Database

                        //sheet2new.Add(details);

                        //_context.Clients.Add(details);
                        //_context.SaveChanges();
                        //StoppedClient stoppedClient = new StoppedClient();
                        //stoppedClient.ClientId = details.Id;
                        //stoppedClient.Reason = "غير مودع";
                        //stoppedClient.StartDate = DateTime.Now;
                        //_context.StoppedClients.Add(stoppedClient);
                        //_context.SaveChanges();
                        //Penefit clientPenefit = new Penefit();
                        //clientPenefit.ClientId = details.Id;
                        //clientPenefit.CompleteAmount = 0;
                        //_context.Add(clientPenefit);
                        //_context.SaveChanges();
                        //Payment clientPayment = new Payment();
                        //clientPayment.ClientId = details.Id;
                        //clientPayment.Amount = 0;
                        //_context.Add(clientPayment);
                        //_context.SaveChanges();



                        //}
                        //else
                        //{

                        //    if (checkClient.Nationality != details.Nationality || checkClient.NationalId != details.NationalId || checkClient.NationalIdType != details.NationalIdType || checkClient.NotactiveStocks != details.NotactiveStocks || checkClient.ActiveStocks != details.ActiveStocks || checkClient.MiddleName != details.MiddleName || checkClient.Mother != details.Mother || checkClient.LastName != details.LastName || checkClient.ClientTypeId != details.ClientTypeId || checkClient.OriginalAddress != details.OriginalAddress || checkClient.CityId != details.CityId)

                        //    {
                        //        details.Id = checkClient.Id;
                        //        sheet2update.Add(details);
                        //        sheet2beforeupdate.Add(checkClient);
                        //    }



                        //}
                    }

                    //// Read the the Table sheet1 مودع
                    //DataTable serviceDetails1 = ds.Tables[0];
                    //for (int i = 5; i < serviceDetails1.Rows.Count; i++)
                    //{
                    //    Client details = new Client();
                    //    details.OriginalAddress = serviceDetails1.Rows[i][2].ToString();
                    //    var city = _context.Cities.Where(c => c.CityName == serviceDetails1.Rows[i][3].ToString()).FirstOrDefault();
                    //    if (city == null)
                    //    {
                    //        City c = new City();
                    //        c.CityName = serviceDetails1.Rows[i][3].ToString();
                    //        _context.Add(c);
                    //        _context.SaveChanges();
                    //        details.CityId = c.CityId;
                    //    }
                    //    else
                    //    {
                    //        details.CityId = city.CityId;
                    //    }
                    //    details.Nationality = serviceDetails1.Rows[i][5].ToString();
                    //    details.Fax = serviceDetails1.Rows[i][6].ToString();
                    //    details.HomePhone = serviceDetails1.Rows[i][7].ToString();
                    //    details.Mobile1 = serviceDetails1.Rows[i][8].ToString();
                    //    details.Mobile2 = serviceDetails1.Rows[i][9].ToString();
                    //    details.NotactiveStocks = Convert.ToInt32(serviceDetails1.Rows[i][11]);
                    //    details.ActiveStocks = Convert.ToInt32(serviceDetails1.Rows[i][12]);
                    //    details.Mother = serviceDetails1.Rows[i][13].ToString();
                    //    details.MiddleName = serviceDetails1.Rows[i][14].ToString();
                    //    details.FirstName = serviceDetails1.Rows[i][15].ToString();
                    //    details.clientnumber = serviceDetails1.Rows[i][16].ToString();
                    //    string fullName = serviceDetails1.Rows[i][15].ToString();
                    //    string[] names = fullName.Split(' ');
                    //    // string name = names.First();
                    //    string lasName = names.Last();
                    //    details.LastName = lasName;
                    //    details.IsActive = true;
                    //    details.ClientStatus = "Active";
                    //    if (details.MiddleName == "")
                    //    {
                    //        details.MiddleName = "**";
                    //    }
                    //    if (details.Mother == "")
                    //    {
                    //        details.Mother = "**";
                    //    }
                    //    if (details.LastName == "")
                    //    {
                    //        details.LastName = "**";
                    //    }

                    //    if (details.Fax == "")
                    //    {
                    //        details.Fax = "**";
                    //    }
                    //    if (details.NationalId == "")
                    //    {
                    //        details.NationalId = "**";
                    //    }
                    //    if (details.NationalIdType == "")
                    //    {
                    //        details.NationalIdType = "**";
                    //    }
                    //    if (details.Mobile1 == "")
                    //    {
                    //        details.Mobile1 = "**";
                    //    }
                    //    if (details.Mobile2 == "")
                    //    {
                    //        details.Mobile2 = "**";
                    //    }
                    //    if (details.OriginalAddress == "")
                    //    {
                    //        details.OriginalAddress = "**";
                    //    }
                    //    if (details.Nationality == "")
                    //    {
                    //        details.Nationality = "**";
                    //    }
                    //    if (details.HomePhone == "")
                    //    {
                    //        details.HomePhone = "**";
                    //    }
                    //    if (details.clientnumber == "")
                    //    {
                    //        details.clientnumber = "**";
                    //    }



                    //    // Add the record in Database
                    //    var checkClient2 = _context.Clients.Where(c => c.FirstName == serviceDetails1.Rows[i][15].ToString()).FirstOrDefault();
                    //    if (checkClient2 == null)
                    //    {
                    //        sheet1new.Add(details);

                    //        _context.Clients.Add(details);
                    //        _context.SaveChanges();
                    //        Penefit clientPenefit = new Penefit();
                    //        clientPenefit.ClientId = details.Id;
                    //        clientPenefit.CompleteAmount = 0;
                    //        _context.Add(clientPenefit);
                    //        _context.SaveChanges();
                    //        Payment clientPayment = new Payment();
                    //        clientPayment.ClientId = details.Id;
                    //        clientPayment.Amount = 0;
                    //        _context.Add(clientPayment);
                    //        _context.SaveChanges();

                    //    }
                    //    else
                    //    {

                    //        if (checkClient2.Nationality != details.Nationality || checkClient2.Fax != details.Fax || checkClient2.HomePhone != details.HomePhone || checkClient2.Mobile1 != details.Mobile1 || checkClient2.Mobile2 != details.Mobile2 || checkClient2.NotactiveStocks != details.NotactiveStocks || checkClient2.ActiveStocks != details.ActiveStocks || checkClient2.MiddleName != details.MiddleName || checkClient2.Mother != details.Mother || checkClient2.LastName != details.LastName || checkClient2.ClientTypeId != details.ClientTypeId || checkClient2.OriginalAddress != details.OriginalAddress || checkClient2.CityId != details.CityId)
                    //        {
                    //            sheet1beforupdate.Add(checkClient2);
                    //            details.Id = checkClient2.Id;
                    //            sheet1update.Add(details);
                    //        }

                    //    }

                    //}
                }
            }
            return Json("Done");

            // return RedirectToAction("arraysFromExcel", "Clients",new{ sheet1new=JsonConvert.SerializeObject(sheet1new.ToList()), sheet1update, sheet1beforupdate, sheet2new.Count, sheet2update, sheet2beforeupdate});



        }





        public async Task<IActionResult> arraysFromExcel()
        {

            return View();
        }


        public ActionResult EditClient (Client c)
        { 
            //var client =_context.Clients.Where(i=>i.Id== c.Id).FirstOrDefault();
            //int id = client.Id;
            if(c.MiddleName=="") 
            {
                c.MiddleName = "**";
            }
            if (c.Mother == "")
            {
                c.Mother = "**";
            }
            if (c.LastName == "")
            {
                c.LastName = "**";
            }

            if (c.Fax == "")
            {
                c.Fax = "**";
            }
            if (c.NationalId == "")
            {
                c.NationalId = "**";
            }
            if (c.NationalIdType == "")
            {
                c.NationalIdType = "**";
            }
            if (c.Mobile1== "")
            {
                c.Mobile1 = "**";
            }
            if (c.Mobile2 == string.Empty)
            {
                c.Mobile2 = "**";
            }
            if (c.OriginalAddress == "")
            {
                c.OriginalAddress = "**";
            }
            if (c.Nationality == "")
            {
                c.Nationality = "**";
            }
            if (c.HomePhone == "")
            {
                c.HomePhone = "**";
            }

            _context.Update(c);
            _context.SaveChanges();
            return Json(c);
        }


        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,Mother,NationalId,NationalIdType,Mobile1,Mobile2,AddressDomicil,AddressWork,OriginalAddress,Nationality,ClientStatus,IsActive,HomePhone,Fax,Khana,Birthdate,Birthcity,ClientTypeId,BankAccount,Idphoto,IdcardPhoto,Notes,IsAlive,IsDeleted,CityId,ActiveStocks,NotactiveStocks")] Client client, IFormFile Idphoto, IFormFile IdcardPhoto)
        {
            if (id != client.Id)
            {
                return NotFound();
            }
            //if (_context.Clients.Any(x => x.NationalId == client.NationalId))
            //{
            //    ViewBag.messge = "هذا الرقم الوطني موجود مسبقا";
            //    ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", client.CityId);
            //    ViewData["ClientTypeId"] = new SelectList(_context.ClientTypes, "TypeId", "TypeName", client.ClientTypeId);
            //    return View(client);
            //}
            else
            {

                if (Idphoto != null && IdcardPhoto == null)
                {
                    foreach (var file in Request.Form.Files)
                    {


                        MemoryStream ms = new MemoryStream();
                        file.CopyTo(ms);
                        client.Idphoto = ms.ToArray();

                        ms.Close();
                        ms.Dispose();



                    }
                    //  _context.Update(client);



                    var existingClient = _context.Clients.Find(id);
                    existingClient.Mobile1 = client.Mobile1;
                    existingClient.Mobile2 = client.Mobile2;
                    existingClient.IsDeleted = client.IsDeleted;
                    existingClient.Notes = client.Notes;
                    existingClient.AddressWork = client.AddressWork;
                    existingClient.ActiveStocks = client.ActiveStocks;
                    existingClient.AddressDomicil = client.AddressDomicil;
                    existingClient.Idphoto = client.Idphoto;
                    existingClient.HomePhone = client.HomePhone;
                    // existingClient.IdcardPhoto = client.IdcardPhoto;
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


                    // if (existingClient.FirstName != null && existingClient.LastName != null && existingClient.MiddleName != null && existingClient.BankAccount != null && existingClient.HomePhone != null && existingClient.OriginalAddress != null && existingClient.NationalId != null&&existingClient.ActiveStocks!=null&&existingClient.NotactiveStocks!=null)
                    if (existingClient.FirstName != null && existingClient.LastName != null && existingClient.MiddleName != null && existingClient.Mother != null && existingClient.HomePhone != null || existingClient.OriginalAddress != null && existingClient.Mobile1 != null && existingClient.Mobile2 != null && existingClient.Nationality != null && existingClient.Fax != null && existingClient.CityId != null && existingClient.ActiveStocks != null && existingClient.NotactiveStocks != null && existingClient.NationalId != null)

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
                    //  _context.Update(client);

                    var existingClient = _context.Clients.Find(id);
                    existingClient.Mobile1 = client.Mobile1;
                    existingClient.Mobile2 = client.Mobile2;
                    existingClient.IsDeleted = client.IsDeleted;
                    existingClient.Notes = client.Notes;
                    existingClient.AddressWork = client.AddressWork;
                    existingClient.ActiveStocks = client.ActiveStocks;
                    existingClient.AddressDomicil = client.AddressDomicil;
                    existingClient.HomePhone = client.HomePhone;
                    existingClient.IdcardPhoto = client.IdcardPhoto;
                    // existingClient.Idphoto = client.Idphoto;
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
                    //  if (existingClient.FirstName != null && existingClient.LastName != null && existingClient.MiddleName != null && existingClient.BankAccount != null && existingClient.HomePhone != null && existingClient.OriginalAddress != null && existingClient.NationalId != null && existingClient.ActiveStocks != null && existingClient.NotactiveStocks != null)
                    if (existingClient.FirstName != null && existingClient.LastName != null && existingClient.MiddleName != null && existingClient.Mother != null && existingClient.HomePhone != null || existingClient.OriginalAddress != null && existingClient.Mobile1 != null && existingClient.Mobile2 != null && existingClient.Nationality != null && existingClient.Fax != null && existingClient.CityId != null && existingClient.ActiveStocks != null && existingClient.NotactiveStocks != null && existingClient.NationalId != null)

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

                if (Idphoto != null && IdcardPhoto != null)
                {
                    foreach (var file in Request.Form.Files)
                    {

                        //  img.ImageTitle = file.FileName;

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

                    // if (client.FirstName != null && client.LastName != null && client.MiddleName != null && client.BankAccount != null && client.HomePhone != null && client.OriginalAddress != null && client.NationalId != null && client.ActiveStocks != null && client.NotactiveStocks != null)
                    if (client.FirstName != null && client.LastName != null && client.MiddleName != null && client.Mother != null && client.HomePhone != null || client.OriginalAddress != null && client.Mobile1 != null && client.Mobile2 != null && client.Nationality != null && client.Fax != null && client.CityId != null && client.ActiveStocks != null && client.NotactiveStocks != null && client.NationalId != null)

                    {
                        client.ClientStatus = "Active";






                        var clientstopped = _context.StoppedClients.Where(c => c.ClientId == client.Id).First();
                        if (clientstopped != null)
                        {
                            clientstopped.Enddate = DateTime.Now;
                            _context.Update(clientstopped);
                            _context.SaveChanges();
                        }


                    }


                    _context.Update(client);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));

                }

                else
                {
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
                    if (existingClient.FirstName != null && existingClient.LastName != null && existingClient.MiddleName != null && existingClient.Mother != null && existingClient.HomePhone != null || existingClient.OriginalAddress != null && existingClient.Mobile1 != null && existingClient.Mobile2 != null && existingClient.Nationality != null && existingClient.Fax != null && existingClient.CityId != null && existingClient.ActiveStocks != null && existingClient.NotactiveStocks != null && existingClient.NationalId != null)

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

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.City)
                .Include(c => c.ClientType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'AhliahContext.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return _context.Clients.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ActiveClients()
        {
            var ahliahContext = _context.Clients.Where(c => c.ClientStatus == "Active")
                .Include(c => c.City).Include(c => c.ClientType);
            return View(await ahliahContext.ToListAsync());
        }
        public async Task<IActionResult> Index()
        {
           


            var sub = from od in _context.Penefits
                      join q in _context.Clients on od.ClientId equals q.Id
                     // join pay in _context.Payments on q.Id equals pay.ClientId
                     
                      group new { od} by new { od.ClientId }
                        into v
                      select new
                      {
                          cId = v.Key.ClientId,


                          penefitvalue = v.Sum(x => x.od.CompleteAmount),
                         // paymentvalue = v.Sum(x => x.pay.Amount)

                      };
            var sub1 = from pay in _context.Payments
                      join q in _context.Clients on pay.ClientId equals q.Id
                     
                      group new { pay} by new { pay.ClientId }
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



                        select new selectModel
                        {
                            Id = client.Id,
                            total = vs.penefitvalue - vs1.paymentvalue,
                            FirstName = client.FirstName,
                            LastName = client.LastName,
                            MiddleName = client.MiddleName,
                            Mother = client.Mother,
                            OriginalAddress = client.OriginalAddress,
                            Mobile1 = client.Mobile1,
                            TotalStocks=client.ActiveStocks+client.NotactiveStocks,
                            NationalId=client.NationalId,
                            IsAlive=client.IsAlive,
                            clientnumber=client.clientnumber,
                            NotactiveStocks=client.NotactiveStocks,
                            ActiveStocks=client.ActiveStocks

                            
                        }
                        ;
            var x = query.ToList();






            return View(query);


        }


        public async Task<IActionResult> Archive()
        {



            var sub = from od in _context.Penefits
                      join q in _context.Clients on od.ClientId equals q.Id
                   

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

                        where vs1.paymentvalue!=0

                        select new selectModel
                        {
                            Id = client.Id,
                            total = vs.penefitvalue - vs1.paymentvalue,
                            FirstName = client.FirstName,
                            LastName = client.LastName,
                            MiddleName = client.MiddleName,
                            Mother = client.Mother,
                            OriginalAddress = client.OriginalAddress,
                            Mobile1 = client.Mobile1,
                            TotalStocks = client.ActiveStocks + client.NotactiveStocks,
                            NationalId = client.NationalId,
                            IsAlive = client.IsAlive,
                            clientnumber = client.clientnumber,
                            NotactiveStocks = client.NotactiveStocks,
                            ActiveStocks = client.ActiveStocks


                        }
                        ;
            var x = query.ToList();






            return View(query);


        }
        public async Task<IActionResult> backupAsync()
        {




            string connstr = configuration.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(connstr);
            connection.Open();

            // string backupDIR = "E:\\BackupDB";
            string backupDIR = Path.Combine(hostEnvironment.WebRootPath, "backup");
            string filename = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string filename1 = filename + ".Bak";
            if (!System.IO.Directory.Exists(backupDIR))
            {
                System.IO.Directory.CreateDirectory(backupDIR);
            }
            //  string sql = string.Format("BACKUP DATABASE Ahliah  To Disk='{0}' WITH STATS", path);

            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand($"backup database Ahliah to disk='{Path.Combine(backupDIR, filename1)}'", connection);

            sqlcmd.ExecuteNonQuery();
            connection.Close();
            Thread.Sleep(5000);

            // var memory = DownloadSinghFile(filename, backupDIR);
            string filepth =Path.Combine(backupDIR, filename1);

            string mimeType = MimeTypes.GetMimeType(filename1);
            Console.WriteLine("11");
            FileStream fileStream = new FileStream(filepth, FileMode.Open, FileAccess.Read);
            fileStream.Position = 0;
            return File(fileStream, mimeType, filename1);

            // return File(memory.ToArray(), mimeType, filename);


            //  //        var path = Path.Combine(
            //  //Directory.GetCurrentDirectory(), "wwwroot\\images\\4.pdf");


            //  Thread.Sleep(8000);
            //  //var path = backupDIR + "\\" + filename;
            //  var path = Path.Combine(
            //Directory.GetCurrentDirectory(), backupDIR,filename);

            //var memory = new MemoryStream();
            //  using (var stream = new FileStream(path, FileMode.Open))
            //  {
            //      await stream.CopyToAsync(memory);
            //  }
            //  memory.Position = 0;
            //  return File(memory, "application/Bak", filename);



        }



        public async Task<IActionResult> RetrieveBackupAsync( IFormFile file1)
        {



            string dataFileName = Path.GetFileName(file1.FileName);


            //string FilePath = Path.Combine(hostingEnv.WebRootPath, "ProductList");

            //string FileNameWithPath = Path.Combine(FilePath, Name);

            string connstr = configuration.GetConnectionString("restoreBackupconnection");
            SqlConnection connection = new SqlConnection(connstr);
            connection.Open();

            // string backupDIR = "E:\\BackupDB";
            string backupDIR = Path.Combine(hostEnvironment.WebRootPath, "backup");
            string FileNameWithPath = Path.Combine(backupDIR, dataFileName);
            SqlCommand sqlcmd1 = new SqlCommand();
            sqlcmd1 = new SqlCommand("alter database Ahliah set offline with rollback immediate", connection);

            sqlcmd1.ExecuteNonQuery();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand($"RESTORE DATABASE Ahliah FROM DISK = '{Path.Combine(backupDIR, dataFileName)}'WITH REPLACE", connection);

            sqlcmd.ExecuteNonQuery();
            SqlCommand sqlcmd2 = new SqlCommand();
            sqlcmd2 = new SqlCommand("alter database Ahliah  set online", connection);

            sqlcmd2.ExecuteNonQuery();
            connection.Close();
            Thread.Sleep(5000);

            // var memory = DownloadSinghFile(filename, backupDIR);
            return RedirectToAction("backupView");




        }

        private MemoryStream DownloadSinghFile(string filename, string uploadPath)
        {
            //var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
            var path = Path.Combine(uploadPath , filename);
            var memory = new MemoryStream();
            if (System.IO.File.Exists(path))
            {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new System.IO.MemoryStream(data);
                memory = content;
            }
            memory.Position = 0;
            return memory;
        }


        public async Task<IActionResult> backupView()
        {


            return View();


        }
        public IActionResult findclientbyname() 
        {
            var clientnumber="0";
            var client = _context.Clients.ToList();
          for(int i = 0; i <= client.Count; i++) 
            {
                string clinentnumber = client[i].clientnumber;
                string newone = clientnumber.Replace("-", "");
                client[i].clientnumber = newone;
                _context.Update(client[i]);
                _context.SaveChanges();

              

            }
            return Json("Done");

        }



    }
}
