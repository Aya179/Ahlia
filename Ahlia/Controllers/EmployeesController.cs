using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ahlia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Ahlia.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly AhliahContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public EmployeesController(AhliahContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var ahliahContext = _context.Employees.Include(e => e.City);
            return View(await ahliahContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeName,UserName,Password,Email,Mobile,RoleId,IsDeleted,CityId,Address,EmpId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", employee.CityId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", employee.CityId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeName,UserName,Password,Email,Mobile,RoleId,IsDeleted,CityId,Address,EmpId")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", employee.CityId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'AhliahContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            IdentityUser user = _userManager.Users.Where(u => u.Id == employee.EmpId).FirstOrDefault();
            var rolesForUser = await _userManager.GetRolesAsync(user);

            if (employee != null)
            {
                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await _userManager.RemoveFromRoleAsync((IdentityUser)user, item);
                    }
                }
                await _userManager.DeleteAsync((IdentityUser)user);
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return _context.Employees.Any(e => e.Id == id);
        }


        public IActionResult CreateRole()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string rolename)
        {
            if (ModelState.IsValid)
            {
               
                bool x = await _roleManager.RoleExistsAsync(rolename);
                if (!x)
                {
                    // first we create Admin rool    
                    var role = new IdentityRole();
                    role.Name = rolename;
                    await _roleManager.CreateAsync(role);
                }

                return RedirectToAction(nameof(IndexRoleView));
            }
            return View(rolename);
        }


        public async Task<IActionResult> IndexRoleView()
        {
           
            
            return View();
        }
        public async Task<IActionResult> IndexRole()
        {
           // List<string> roles = roleMngr.Roles.Select(x => x.Name).ToList();
            var ahliahContext = _roleManager.Roles.ToList();
            return Json(ahliahContext);
        }

        
        public async Task<IActionResult> deleteRoleView(string id)
        {
            var currentRole = _roleManager.Roles.Where(e => e.Id == id).FirstOrDefault();
            ViewBag.name = currentRole.Name;
            ViewBag.roleid = currentRole.Id;


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> deleteRoleView(string id,string name)
        {
           var currentRole=_roleManager.Roles.Where(e => e.Id == id).FirstOrDefault();
            var role = await _roleManager.FindByNameAsync(currentRole.Name);
            var result = await _roleManager.DeleteAsync(role);
           
            return RedirectToAction(nameof(IndexRoleView));
        } 
        public async Task<IActionResult> updateRoleView(string id)
        {
            var currentRole = _roleManager.Roles.Where(e => e.Id == id).FirstOrDefault();
            ViewBag.name = currentRole.Name;
            ViewBag.roleid = currentRole.Id;


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> updateRoleView(string id,string name)
        {
           var currentRole=_roleManager.Roles.Where(e => e.Id == id).FirstOrDefault();
            var role = await _roleManager.FindByNameAsync(currentRole.Name);
            role.Name = name;
            var result = await _roleManager.UpdateAsync(role);
           
            return RedirectToAction(nameof(IndexRoleView));
        }

    }
}
