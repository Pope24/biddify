using Biddify.SignalR;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Service;
using System.Threading.Tasks;

namespace Biddify.Pages.Auction;

public class AuctionDetailModel : PageModel
{
    private readonly IAuctionProductService auctionProductService;
    private readonly UserManager<UserEntity> userManager;
    private readonly IBidService bidService;
    private readonly IHubContext<BidHub> bidHubContext;
    public AuctionDetailModel(IAuctionProductService _auctionProductService, UserManager<UserEntity> _userManager, IBidService _bidService, IHubContext<BidHub> _bidHubContext)
    {
        this.auctionProductService = _auctionProductService;
        this.userManager = _userManager;
        this.bidService = _bidService;
        this.bidHubContext = _bidHubContext;
    }

    public AuctionProductEntity Item { get; set; }
    public List<AuctionProductEntity> AuctionItems { get; set; }

    public async Task OnGetAsync(string id)
    {
        Item = await auctionProductService.GetAuctionProductByIdAsync(id);
        var twoHours = DateTime.UtcNow.AddHours(2);

        AuctionItems = (List<AuctionProductEntity>)await auctionProductService.GetAuctionProductsAsync();
    }
    public async Task<IActionResult> OnPostBidAsync(decimal AmountBid, string ItemId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToPage("/Authen/Login");

        Item = await auctionProductService.GetAuctionProductByIdAsync(ItemId);
        // Check amountBid có đúng không
        var currentPrice = Item.Bids.Count() == 0 ? Item.StartPrice : Item.Bids.OrderByDescending(b => b.Amount).First().Amount;
        var minBid = Item.MinBidPrice;
        if (AmountBid < currentPrice + minBid)
        {
            ModelState.AddModelError(string.Empty, "Bid too low.");
            return Page();
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

}
