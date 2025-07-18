using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository;
using System.Diagnostics;

namespace Biddify.Pages.Profiles.PartialProfile
{
    public class ProfileInforModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<UserEntity> _userManager;
        public ProfileInforModel(IUserRepository userRepository, UserManager<UserEntity> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }


        [BindProperty]
        public UserEntity UserInfo { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdentity = await _userManager.GetUserAsync(User);


            //var user = await _userRepository.GetUserByEmailAsync(userIdentity.Email);

            return Partial("_ProfileInfor", userIdentity); 
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var existingUser = await _userRepository.GetUserByEmailAsync(UserInfo.Email);
            //if (existingUser == null)
            //    return NotFound();

            //existingUser.DisplayName = UserInfo.DisplayName;
            //existingUser.Email = UserInfo.Email;

            var success = await _userRepository.UpdateUserAsync(UserInfo);
            if (!success)
                return BadRequest("Update failed.");

            return Partial("_ProfileInfor", UserInfo);
        }

    }

}
