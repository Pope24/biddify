using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Threading.Tasks;

namespace Biddify.Pages.Auction;

public class AuctionDetailModel : PageModel
{
    private readonly IAuctionProductService auctionProductService;

    public AuctionDetailModel(IAuctionProductService _auctionProductService)
    {
        this.auctionProductService = _auctionProductService;
    }

    public AuctionProductEntity Item { get; set; }
    public List<AuctionProductEntity> AuctionItems { get; set; }

    public async Task OnGet(string id)
    {
        Item = await auctionProductService.GetAuctionProductByIdAsync(id);
        var twoHours = DateTime.UtcNow.AddHours(2);

        AuctionItems = (List<AuctionProductEntity>)await auctionProductService.GetAuctionProductsAsync();
    }
}
