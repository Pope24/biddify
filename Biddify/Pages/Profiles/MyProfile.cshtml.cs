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
                case "won":
                    return Partial("Partial/_MyWonProducts");
                case "products":
                    return Partial("Partial/_MyProducts");
                case "settings":
                    return Partial("Partial/_Settings");
                default:
                    return Content("Không tìm thấy nội dung.");
            }
        }

    }
}
