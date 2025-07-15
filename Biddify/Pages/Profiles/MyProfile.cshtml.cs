using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Service;

namespace Biddify.Pages.Profiles
{
    public class MyProfileModel : PageModel
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IPaymentService paymentService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserRepository _userRepository;
        public UserEntity CurrentUser { get; set; }

        public MyProfileModel(UserManager<UserEntity> userManager, IPaymentService _paymentService, IHttpContextAccessor contextAccessor, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            paymentService = _paymentService;
            _contextAccessor = contextAccessor;
        }
        [BindProperty]
        public decimal DepositAmount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userRepository.GetUserByEmailAsync(User.Identity.Name);

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
        public async Task<IActionResult> OnPostRequestDepositAsync()
        {
            string description = "Deposit via PayOS";
            var request = _contextAccessor.HttpContext?.Request;
            var checkoutUrl = await paymentService.CreatePaymentLinkOS(DepositAmount, description, $"{request?.Scheme}://{request?.Host}/payment/response");
            return Redirect(checkoutUrl);
        }
    }
}
