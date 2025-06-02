using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biddify.Pages.Home
{
    public class HomeModel : PageModel
    {



        public List<UpcomingAuction> UpcomingAuctions { get; set; }

        public List<ProductBid> ComputerBids { get; set; }
        public List<Winner> Winners { get; set; }
     
  

        public void OnGet()
        {
            
            Winners = new List<Winner>
    {
        new Winner { Name = "Brooklyn Simmons", AvatarUrl = "https://randomuser.me/api/portraits/men/1.jpg", Id = "0195610" },
        new Winner { Name = "Dianne Russell", AvatarUrl = "https://randomuser.me/api/portraits/women/2.jpg", Id = "0195611" },
        new Winner { Name = "Cameron Williamson", AvatarUrl = "https://randomuser.me/api/portraits/men/3.jpg", Id = "0195612" },
        new Winner { Name = "Jane Cooper", AvatarUrl = "https://randomuser.me/api/portraits/women/4.jpg", Id = "0195613" }
    };

          
            ComputerBids = new List<ProductBid>
    {
        new ProductBid { Title = "Flash Sale Pentium Gold G5420 8th Gen Special PC", BidAmount = "$85.00", ImageUrl = "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/p-sm-1.png" },
        new ProductBid { Title = "High-Performance Ryzen Gaming PC", BidAmount = "$120.00", ImageUrl = "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/p-sm-1.png" },
        new ProductBid { Title = "Intel Core i7 12th Gen Tower", BidAmount = "$140.00", ImageUrl = "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/p-sm-1.png" },
        new ProductBid { Title = "Basic Office Desktop PC Package", BidAmount = "$60.00", ImageUrl = "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/p-sm-1.png" }
    };
            UpcomingAuctions = new List<UpcomingAuction>
        {
            new UpcomingAuction
            {
                Title = "Xiaomi Amazfit A1919D Touch Screen Display Smart Watch",
                AuctionId = "20202025463695",
                BidAmount = "$1000.99",
                ImageUrl = "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/p-gamma1.png",
                TimeLeft = "2 : 38 : 10"
            },
             new UpcomingAuction
            {
                Title = "Xiaomi Amazfit A1919D Touch Screen Display Smart Watch",
                AuctionId = "20202025463695",
                BidAmount = "$1000.99",
                ImageUrl = "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/p-gamma1.png",
                TimeLeft = "2 : 38 : 10"
            },
              new UpcomingAuction
            {
                Title = "Xiaomi Amazfit A1919D Touch Screen Display Smart Watch",
                AuctionId = "20202025463695",
                BidAmount = "$1000.99",
                ImageUrl = "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/p-gamma1.png",
                TimeLeft = "2 : 38 : 10"
            },
               new UpcomingAuction
            {
                Title = "Xiaomi Amazfit A1919D Touch Screen Display Smart Watch",
                AuctionId = "20202025463695",
                BidAmount = "$1000.99",
                ImageUrl = "https://demo-egenslab.b-cdn.net/html/bidgen/preview/assets/images/product/p-gamma1.png",
                TimeLeft = "2 : 38 : 10"
            },

        };
        }

    }
    public class Winner
    {
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Id { get; set; }
    }
    public class ProductBid
    {
        public string Title { get; set; }
        public string BidAmount { get; set; }
        public string ImageUrl { get; set; }
    }
    public class UpcomingAuction
    {
        public string Title { get; set; }
        public string AuctionId { get; set; }
        public string BidAmount { get; set; }
        public string ImageUrl { get; set; }
        public string TimeLeft { get; set; }
    }
}
