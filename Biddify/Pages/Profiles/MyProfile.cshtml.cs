using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biddify.Pages.Profiles
{
    public class MyProfileModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnGetLoadPartialAsync(string tab)
        {
            switch (tab)
            {
                case "info":
                    return Partial("Partial/_ProfileInfor");
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
