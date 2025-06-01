using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BidEntity
    {
        public string Id { get; set; }
        public string AuctionProductId { get; set; }
        public AuctionProductEntity AuctionProduct { get; set; }
        public string BidderId { get; set; }
        public UserEntity Bidder { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
