using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biddify.Pages.Profiles
{
    public class MyProfileModel : PageModel
    {
        private readonly UserManager<UserEntity> _userManager;

        public UserEntity CurrentUser { get; set; }

        public MyProfileModel(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);

            return Page();
        }
        public async Task<IActionResult> OnGetLoadPartialAsync(string tab)
        {
            var user = await _userManager.GetUserAsync(User);
            switch (tab)
            {
                case "info":
                    return Partial("Partial/_ProfileInfor", user);
                case "security":
                    return Partial("Partial/_SecurityManagement");
                case "auction":
                    return Partial("Partial/_HistoryAuction");
                case "transaction":
                    return Partial("Partial/_HistoryTransaction");
             
                default:
                    return Content("Không tìm thấy nội dung.");
            }
        }

    }
}
