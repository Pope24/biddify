using Common;
using DataAccess;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.payOS;
using Newtonsoft.Json;
using Service;
using System.Text;

namespace Biddify.Pages.Payment
{
    public class ResponseModel : PageModel
    {
        private readonly PayOS _payOS;
        private readonly ITransactionService transactionService;
        private readonly UserManager<UserEntity> userManager;
        private readonly IWinningService winningService;

        public ResponseModel(IConfiguration config, PayOS payOS, UserManager<UserEntity> _userManager, ITransactionService _transactionService, IWinningService _winningService)
        {
            _payOS = payOS;
            transactionService = _transactionService;
            userManager = _userManager;
            winningService = _winningService;
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
        public DateTime TransactionTime { get; set; } = DateTime.Now;

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var transaction = await transactionService.GetTransactionByCodeAsync(OrderCode);
            if (transaction == null)
            {
                return Redirect("/");
            }
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return Redirect("/");
            }

            var inforWebhook = await _payOS.getPaymentLinkInformation(OrderCode);
            Amount = inforWebhook.amount;
            TransactionTime = DateTimeOffset.Parse(inforWebhook.createdAt).UtcDateTime;


            var typeTransaction = transaction.Type;
            var auctionProductId = transaction?.Metadata?.AuctionProductId;
            
            if (inforWebhook.status != "CANCELLED")
            {
                user.Balance += ((decimal)Amount / 26018);
                Message = "Thank you! Your payment was completed successfully. Please check your balance into account profile.";

                transaction.Status = ETransactionStatus.Success;

                if (typeTransaction == ETransactionType.Payment && !string.IsNullOrEmpty(auctionProductId)) {
                    var winner = await winningService.GetWinnerByAuctionIdAsync(auctionProductId);

                    winner.IsPaid = true;
                    await winningService.UpdateWinnerAsync(winner);
                }
            }
            else
            {
                Message = "Sorry, your payment failed or was cancelled.";
                transaction.Status = ETransactionStatus.Cancelled;
            }

            await transactionService.UpdateTransactionAsync(transaction);
            return Page();
        }
    }
}
