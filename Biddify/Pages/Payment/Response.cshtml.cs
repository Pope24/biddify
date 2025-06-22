using DataAccess;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.payOS;
using Service;

namespace Biddify.Pages.Payment
{
    public class ResponseModel : PageModel
    {
        private readonly PayOS _payOS;
        private readonly ITransactionService transactionService;
        private readonly UserManager<UserEntity> userManager;

        public ResponseModel(IConfiguration config, PayOS payOS, UserManager<UserEntity> _userManager, ITransactionService _transactionService)
        {
            _payOS = payOS;
            transactionService = _transactionService;
            userManager = _userManager;
        }
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Code { get; set; }

        [BindProperty(SupportsGet = true)]
        public long OrderCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Amount { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Description { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime TransactionTime { get; set; } = DateTime.Now;

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (await transactionService.IsExistTransactionAsync(OrderCode))
            {
                return Redirect("/");
            }
            var user = await userManager.GetUserAsync(User);
            var inforWebhook = await _payOS.getPaymentLinkInformation(OrderCode);
            Amount = inforWebhook.amount;
            TransactionTime = DateTimeOffset.Parse(inforWebhook.createdAt).UtcDateTime;
            if (Code == "00")
            {
                user.Balance += ((decimal)Amount / 26018);
                Message = "Thank you! Your payment was completed successfully. Please check your balance into account profile.";
                await transactionService.AddTransactionAsync(Id, OrderCode, user.Id, ETransactionType.Deposit, ETransactionStatus.Success, Amount, TransactionTime);
                await userManager.UpdateAsync(user);
            }
            else
            {
                Message = "Sorry, your payment failed or was cancelled.";
                await transactionService.AddTransactionAsync(Id, OrderCode, user.Id, ETransactionType.Deposit, ETransactionStatus.Cancelled, Amount, TransactionTime);
            }
            return Page();
        }
    }
}
