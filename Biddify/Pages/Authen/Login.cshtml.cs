using DataAccess;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace Biddify.Pages.Authen
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<UserEntity> signInManager;
        private readonly UserManager<UserEntity> userManager;
        private readonly IEmailService emailService;
        public LoginModel(SignInManager<UserEntity> _signInManager, IEmailService _emailService, UserManager<UserEntity> _userManager)
        {
            signInManager = _signInManager;
            emailService = _emailService;
            userManager = _userManager;
        }
        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var res = await signInManager.PasswordSignInAsync(LoginViewModel.Email, LoginViewModel.Password, LoginViewModel.RememberMe, false);
                if (res.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(LoginViewModel.Email);
                    if (user.EmailConfirmed == false || user.Status == EUserStatus.Inactive)
                    {
                        if (await emailService.IsEmailVerifiedAsync(user.Email))
                        {
                            user.EmailConfirmed = true;
                            user.Status = EUserStatus.Active;
                            await userManager.UpdateAsync(user);
                            return RedirectToPage("/Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Your account have not active, please check email to verify email");
                            await emailService.VerifyEmailAddressAsync(user.Email);
                            return Page();
                        }
                    }
                    return RedirectToPage("/Index");
                }
                ModelState.AddModelError("", "Email or password is incorrect");
            }
            return Page();
        }
    }
}
