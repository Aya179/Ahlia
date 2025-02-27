using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Ahlia.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {


        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var newUser = _userManager.Users.Where(i => i.Email == Input.Email).FirstOrDefault();

                if (newUser != null &&
                    await _userManager.CheckPasswordAsync(newUser, Input.Password))
                {
                    var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, newUser.UserName));
                    //identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    //identity.AddClaim(new Claim(ClaimTypes, user.UserName));


                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
                        new ClaimsPrincipal(identity));


                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user.Email != null)
                    {
                        return Redirect("~/Home/Index");
                    }
                    return LocalRedirect(returnUrl);
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                //if (result.Succeeded)
                //{
                //    _logger.LogInformation("User logged in.");


                //    var user = await _userManager.FindByNameAsync(Input.Email);
                //    if (user.Email != null)
                //    {
                //        return Redirect("~/Home/Index");
                //    }

                //    return LocalRedirect(returnUrl);
                  
                //}
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


    }
}
