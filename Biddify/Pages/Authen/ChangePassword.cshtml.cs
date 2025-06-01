using DataAccess;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biddify.Pages.Authen
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<UserEntity> _userManager;

        public ChangePasswordModel(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }
        [BindProperty]
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["ErrorTokenMessage"] = "Can not find user ID or token.";
                return Page();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorTokenMessage"] = "Can not find user by ID.";
            }
            ChangePasswordViewModel = new ChangePasswordViewModel();
            ChangePasswordViewModel.Email = user.Email;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(ChangePasswordViewModel.Email);
                if (user != null) 
                { 
                    var result = await _userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        await _userManager.AddPasswordAsync(user, ChangePasswordViewModel.NewPassword);
                    }
                }
            }
            else
            {
                return Page();
            }
                return RedirectToPage("/Authen/Login");
        }
    }
}
