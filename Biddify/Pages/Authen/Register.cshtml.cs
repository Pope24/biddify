using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccess;
using Domain.Models;
using Domain.Enums;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Amazon.SimpleEmail;
using Service;

namespace Biddify.Pages.Authen
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailService emailService;

        public RegisterModel(UserManager<UserEntity> userManager, IConfiguration _configuration, IEmailService _emailService)
        {
            this.userManager = userManager;
            this.configuration = _configuration;
            this.emailService = _emailService;
        }
        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                UserEntity user = new UserEntity()
                {
                    DisplayName = RegisterViewModel.Name,
                    Email = RegisterViewModel.Email,
                    UserName = RegisterViewModel.Email,
                    Role = RegisterViewModel.IsSeller ? ERole.Seller : ERole.Bidder
                };
                var res = await userManager.CreateAsync(user, RegisterViewModel.Password);
                if (res.Succeeded)
                {
                    //return RedirectToPage("/Authen/Login");
                    await emailService.VerifyEmailAddressAsync(user.Email);
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return Page();
        }
    }
}
