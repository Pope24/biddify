//using DataAccess;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace Biddify.Pages.Profiles.PartialProfile
//{
//    public class ProfileInforModel : PageModel
//    {
//        private readonly UserManager<UserEntity> _userManager;

//        public ProfileInforModel(UserManager<UserEntity> userManager)
//        {
//            _userManager = userManager;
//        }

//        [BindProperty]
//        public UserEntity UserInfo { get; set; }

//        public async Task<IActionResult> OnGetAsync()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//                return NotFound();

//            UserInfo = user;
//            return Page(); // hoặc Partial nếu dùng trong component
//        }

//        public async Task<IActionResult> OnPostAsync()
//        {
//            if (!ModelState.IsValid)
//                return Page();

//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//                return NotFound();

//            // Cập nhật thông tin
//            user.DisplayName = UserInfo.DisplayName;
//            user.Email = UserInfo.Email;
//            user.UserName = UserInfo.Email; // nếu dùng Email làm UserName

//            var result = await _userManager.UpdateAsync(user);
//            if (!result.Succeeded)
//            {
//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                }
//                return Page();
//            }

//            TempData["SuccessMessage"] = "Profile updated successfully.";
//            return RedirectToPage(); // hoặc return Partial nếu bạn dùng trong component
//        }
//    }
//}
