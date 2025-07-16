using Common;
using DataAccess;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Service;
using Service.Impl;

namespace Biddify.Pages.Profiles
{
    public class MyProfileModel : PageModel
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IPaymentService paymentService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBidService _bidService;
        private readonly ITransactionService _transactionService;
        private readonly IAuctionProductService _auctionProductService;
        private readonly IUserRepository _userRepository;
        public UserEntity CurrentUser { get; set; }

        public MyProfileModel(UserManager<UserEntity> userManager, IPaymentService _paymentService, IHttpContextAccessor contextAccessor, IBidService bidService, IUserRepository userRepository, ITransactionService transactionService, IAuctionProductService auctionProductService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            paymentService = _paymentService;
            _contextAccessor = contextAccessor;
            _bidService = bidService;
            _transactionService = transactionService;
            _auctionProductService = auctionProductService;
        }
        [BindProperty]
        public decimal DepositAmount { get; set; }
        [BindProperty]
        public string TransactionType { get; set; }

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
                    var bids = await _bidService.GetBidsByUserIdAsync(user.Id);
                    return Partial("Partial/_HistoryAuction", bids);
                case "transaction":
                    var transactions = await _transactionService.GetTransactionsByUserIdAsync(user.Id);
                    return Partial("Partial/_HistoryTransaction", transactions);
                default:
                    return Content("Không tìm thấy nội dung.");
            }
        }
        public async Task<IActionResult> OnPostRequestDepositAsync()
        {
            string description = $"{TransactionType} via PayOS";
            var request = _contextAccessor.HttpContext?.Request;

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Page();

            var checkoutUrl = await paymentService.CreatePaymentLinkOS(user.Id, null, TransactionType, DepositAmount, description, $"{request?.Scheme}://{request?.Host}/payment/response");
            return Redirect(checkoutUrl);
        }
        public async Task<PartialViewResult> OnGetAuctionDetailsAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Partial("Profiles/Partial/_AuctionDetailModal", null);
            }

            var auctionProduct = await _auctionProductService.GetAuctionProductByIdAsync(id);
            return Partial("Profiles/Partial/_AuctionDetailModal", auctionProduct);
        }
    }
}
