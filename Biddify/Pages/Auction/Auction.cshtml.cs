using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace Biddify.Pages.Auction;

public class AuctionModel : PageModel
{
    private readonly IAuctionProductService auctionProductService;

    public AuctionModel(IAuctionProductService auctionProductService)
    {
        this.auctionProductService = auctionProductService;
    }

    public List<AuctionProductEntity> AuctionItems { get; set; }

    public async Task OnGetAsync()
    {
        AuctionItems = (List<AuctionProductEntity>)await auctionProductService.GetAuctionProductsAsync();
    }

    public IActionResult OnPostSubmitBid(int ItemId)
    {
        // TODO: xử lý submit bid
        return RedirectToPage();
    }
}