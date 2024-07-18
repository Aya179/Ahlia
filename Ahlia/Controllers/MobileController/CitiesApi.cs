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
    public class CitiesApi : Controller
    {
        private readonly AhliahContext _context;

        public CitiesApi(AhliahContext context)
        {
            _context = context;
        }

       // https://localhost:7204/CitiesApi/AllCities
        
        public async Task<IActionResult> AllCities()
        {
              return Json(await _context.Cities.ToListAsync());
        }





      //  https://localhost:7204/CitiesApi/Create?CityName=tt&Country=sy
        [HttpPost]
        public IActionResult Create(string CityName,string Country )
        {
            
                City city=new City();
                city.CityName=CityName;
                city.Country=Country;

                _context.Add(city);
                 _context.SaveChanges();

                return Json(city);
            
           
        }

        //https://localhost:7204/CitiesApi/Delete?id=23
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Cities == null)
            {
                return Problem("Entity set 'AhliahContext.Cities'  is null.");
            }
            var city =  _context.Cities.Find(id);
            if (city != null)
            {
                _context.Cities.Remove(city);
            }
            
            _context.SaveChanges();
            return Json("تم الحذف");
        }

        private bool CityExists(int id)
        {
          return _context.Cities.Any(e => e.CityId == id);
        }
    }
}
