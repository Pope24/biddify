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
        //var twoHours = DateTime.UtcNow.AddHours(2);

        //AuctionItems = new List<AuctionProductEntity>
        //{
        //    new AuctionItem
        //    {
        //        Id = 1,
        //        Name = "Citroën C4 Picasso",
        //        Price = 2526,
        //        CurrentBid = 1079,
        //        TimeLeft = "11:12:43",
        //        Status = "Waiting for Bid",
        //        ImageUrl =
        //            "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/lp-1.png",
        //        EndTimeUtc = twoHours,
        //    },
        //    new AuctionItem
        //    {
        //        Id = 2,
        //        Name = "Ford Focus 1.2",
        //        Price = 3000,
        //        CurrentBid = 2000,
        //        TimeLeft = "11:12:43",
        //        Status = "Waiting for Bid",
        //        ImageUrl =
        //            "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/lp-2.png",
        //        EndTimeUtc = twoHours.AddMinutes(30),
        //    },
        //    new AuctionItem
        //    {
        //        Id = 3,
        //        Name = "Masrati Mustang GT Premium",
        //        Price = 1500,
        //        CurrentBid = 899,
        //        TimeLeft = "11:12:43",
        //        Status = "Waiting for Bid",
        //        ImageUrl =
        //            "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/lp-3.png",
        //        EndTimeUtc = DateTime.UtcNow.AddHours(9).AddMinutes(40),
        //    },
        //};
    }

    public IActionResult OnPostSubmitBid(int ItemId)
    {
        // TODO: xử lý submit bid
        return RedirectToPage();
    }
}