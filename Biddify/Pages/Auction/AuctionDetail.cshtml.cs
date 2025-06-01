using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biddify.Pages.Auction;

public class AuctionDetailModel : PageModel
{
    public AuctionItem Item { get; set; }
    public List<AuctionItem> AuctionItems { get; set; }

    public void OnGet(int id)
    {
        Item = new AuctionItem
        {
            Id = id,
            Name = "Lamborghini Huracan Red Mercedes AMG",
            Price = 20979.00,
            CurrentBid = 20979.00,
            TimeLeft = "11:00:00:08:23",
            Status = "Live",
            ImageUrl = "/images/car-main.jpg",
            EndTimeUtc = DateTime.UtcNow.AddDays(2),
        };
        var twoHours = DateTime.UtcNow.AddHours(2);

        AuctionItems = new List<AuctionItem>
        {
            new AuctionItem
            {
                Id = 1,
                Name = "Citroën C4 Picasso",
                Price = 2526,
                CurrentBid = 1079,
                TimeLeft = "11:12:43",
                Status = "Waiting for Bid",
                ImageUrl =
                    "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/lp-1.png",
                EndTimeUtc = twoHours,
            },
            new AuctionItem
            {
                Id = 2,
                Name = "Ford Focus 1.2",
                Price = 3000,
                CurrentBid = 2000,
                TimeLeft = "11:12:43",
                Status = "Waiting for Bid",
                ImageUrl =
                    "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/lp-2.png",
                EndTimeUtc = twoHours.AddMinutes(30),
            },
            new AuctionItem
            {
                Id = 3,
                Name = "Masrati Mustang GT Premium",
                Price = 1500,
                CurrentBid = 899,
                TimeLeft = "11:12:43",
                Status = "Waiting for Bid",
                ImageUrl =
                    "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/lp-3.png",
                EndTimeUtc = DateTime.UtcNow.AddHours(9).AddMinutes(40),
            },
        };
    }

    public class AuctionItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double CurrentBid { get; set; }
        public string TimeLeft { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public DateTime EndTimeUtc { get; set; }
    }
}
