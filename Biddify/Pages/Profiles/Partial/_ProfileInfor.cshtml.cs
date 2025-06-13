using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biddify.Pages.Profiles.PartialProfile
{
    public class ProfileInforModel
    {
        [BindProperty]
        public string FirstName { get; set; } = "Musharof";

        [BindProperty]
        public string LastName { get; set; } = "Chowdhury";

        [BindProperty]
        public string Email { get; set; } = "randomuser@pimjo.com";

        [BindProperty]
        public string Phone { get; set; } = "+09 363 398 46";

        [BindProperty]
        public string Bio { get; set; } = "Team Manager";

        public void OnGet()
        {
        }

        //public IActionResult OnPost()
        //{
        //    // Handle form submission logic here (e.g. save to DB)
        //    TempData["Message"] = "Profile updated successfully!";
        //    return RedirectToPage();
        //}

      
    }
}
