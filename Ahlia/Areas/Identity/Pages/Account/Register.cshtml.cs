using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Ahlia.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ahlia.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly AhliahContext _context;

        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, AhliahContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public string? EmployeeName { get; set; }
            public string? Mobile { get; set; }
            public string? RoleId { get; set; }
            public int? CityId { get; set; }
            public string? Address { get; set; }


            // public string roles { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            ViewData["AllRoles"] = new SelectList(_roleManager.Roles, "Id", "Name");

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                Employee employee=new Employee();
                var user = new IdentityUser { UserName = Input.EmployeeName, Email = Input.Email,EmailConfirmed=true };
                var result = await _userManager.CreateAsync(user, Input.Password);
              
            //    var user1 = await _userManager.FindByNameAsync(user.UserName);
              // var roleresult = await _userManager.AddToRoleAsync(user1, Input.roles);
                if (result.Succeeded)
                {
                    employee.Address = Input.Address;
                    employee.UserName = Input.EmployeeName;
                    employee.RoleId = Input.RoleId;
                    employee.CityId = Input.CityId;
                    employee.Email = Input.Email;
                    employee.EmployeeName = Input.EmployeeName;
                    employee.Mobile = Input.Mobile;
                    employee.Password = Input.Password;
                    _context.Employees.Add(employee);
                    var userId = user.Id;
                    employee.EmpId = userId;



                    var Selectedrole = _roleManager.Roles.Where(r => r.Id == Input.RoleId).FirstOrDefault();

                    IdentityResult roleresult = await _userManager.AddToRoleAsync(user, Selectedrole.Name);



                    _context.SaveChanges();


                    // return RedirectToAction("Index", "Home");
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
