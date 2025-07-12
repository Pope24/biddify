using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Biddify.Attributes;

namespace Biddify.Pages.Admin.Dashboard
{
    [AdminAuthorization]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
