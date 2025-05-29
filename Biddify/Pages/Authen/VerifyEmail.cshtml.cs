using DataAccess;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace Biddify.Pages.Authen
{
    public class VerifyEmailModel : PageModel
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly IEmailService emailService;

        public VerifyEmailModel(UserManager<UserEntity> userManager, IEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
        }

        [BindProperty]
        public VerifyEmailViewModel VerifyEmailViewModel { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(VerifyEmailViewModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email is not exist");
                }
                else
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Page(
                        "/Authen/ChangePassword",
                        pageHandler: null,
                        values: new { userId = user.Id, token = token },
                        protocol: Request.Scheme);
                    await emailService.SendEmailAsync(user.Email, confirmationLink);
                    TempData["SuccessMessage"] = "We sent a confirmation email. Please check your inbox and click the link to reset your password.";
                }
            }
            return Page();
        }
    }
}
