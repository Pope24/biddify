using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biddify.Pages.Authen
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<UserEntity> signInManager;

        public LogoutModel(SignInManager<UserEntity> _signInManager)
        {
            this.signInManager = _signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}
