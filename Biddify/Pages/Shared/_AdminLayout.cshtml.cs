using Biddify.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biddify.Pages.Shared
{
    [AdminAuthorization]
    public class _AdminLayoutModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
