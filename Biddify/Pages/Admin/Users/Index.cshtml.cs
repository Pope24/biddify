using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Biddify.Attributes;

namespace Biddify.Pages.Admin.Users
{
    [AdminAuthorization]
    public class IndexModel : PageModel
    {
        public void OnGet() { }
    }
}
