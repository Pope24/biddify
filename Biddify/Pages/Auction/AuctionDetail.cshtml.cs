using Biddify.SignalR;
using Common;
using DataAccess;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Service;

namespace Biddify.Pages.Auction;

public class AuctionDetailModel : PageModel
{
    private readonly IAuctionProductService auctionProductService;
    private readonly UserManager<UserEntity> userManager;
    private readonly IBidService bidService;
    private readonly IHubContext<BidHub> bidHubContext;
    private readonly ICommentService commentService;
    public AuctionDetailModel(IAuctionProductService _auctionProductService, UserManager<UserEntity> _userManager, IBidService _bidService, IHubContext<BidHub> _bidHubContext, ICommentService _commentService)
    {
        this.auctionProductService = _auctionProductService;
        this.userManager = _userManager;
        this.bidService = _bidService;
        this.bidHubContext = _bidHubContext;
        this.commentService = _commentService;
    }

    public AuctionProductEntity Item { get; set; }
    public List<AuctionProductEntity> AuctionItems { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        Item = await auctionProductService.GetAuctionProductByIdAsync(id);
        AuctionItems = (List<AuctionProductEntity>)await auctionProductService.GetAuctionProductsAsync();
        return Page();
    }
    public async Task<IActionResult> OnPostBidAsync(decimal AmountBid, string ItemId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToPage("/Authen/Login");

        Item = await auctionProductService.GetAuctionProductByIdAsync(ItemId);
        if (Item.Status != EAuctionStatus.Active)
        {
            TempData["AccessBidDetailError"] = $"Auction product is '{EnumHelper.ToDisplayString(Item.Status)}' status.";
            return RedirectToPage();
        }
        var now = DateTime.UtcNow;

        if (now < Item.StartTime || now > Item.EndTime)
        {
            TempData["AccessBidDetailError"] = "The product is not in the auction period time.";
            return RedirectToPage();
        }
        var requiredFee = Item.StartPrice * 0.01m;
        if (user.Balance < requiredFee)
        {
            TempData["AccessBidDetailError"] = $"You need at least {requiredFee:#,##0.00}$ (1%) to participate in the auction.";
            return RedirectToPage();
        }
        // Check amountBid có đúng không
        var currentPrice = Item.Bids.Count() == 0 ? Item.StartPrice : Item.Bids.OrderByDescending(b => b.Amount).First().Amount;
        var minBid = Item.MinBidPrice;
        if (AmountBid < currentPrice + minBid)
        {
            TempData["AccessBidDetailError"] = $"You need to bid at least {(currentPrice + minBid):#,##0.00}$.";
            return RedirectToPage();
        }

        var newBid = new BidEntity
        {
            Id = Guid.NewGuid().ToString(),
            BidderId = user.Id,
            AuctionProductId = Item.Id,
            Amount = AmountBid
        };
        await bidService.AddBidAsync(newBid);
        await bidHubContext.Clients.All.SendAsync("ReceiveBidAdd", ItemId.ToString());
        return RedirectToPage();
    }
    public async Task<IActionResult> OnGetBidInfo(string id)
    {
        var item = await auctionProductService.GetAuctionProductByIdAsync(id);
        var bid = item.Bids.OrderByDescending(b => b.CreatedAt).FirstOrDefault();
        return new JsonResult(new
        {
            current = bid?.Amount ?? item.StartPrice,
            bids = item.Bids
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new {
                    name = b.Bidder.DisplayName,
                    created = b.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                    amount = b.Amount.ToString("#,##0.00")
                })
        });
    }
    public async Task<IActionResult> OnGetCommentsAsync(string auctionId, int page = 1, int pageSize = 10)
    {
        var comments = (await commentService.GetCommentLazyLoadingAsync(auctionId, page, pageSize))
            .Select(c => new
            {
                user = c.User.DisplayName,
                message = c.Content,
                createAt = DateTimeExtension.ConvertUtcToVNTime(c.CreatedAt).ToString("dd/MM/yyyy HH:mm:ss")
            });

        return new JsonResult(comments);
    }
}
