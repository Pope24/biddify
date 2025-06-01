using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class WinningEntity
    {
        public string Id { get; set; }

        public string AuctionProductId { get; set; }
        public AuctionProductEntity AuctionProduct { get; set; }
        public string WinnerId { get; set; }
        public UserEntity Winner { get; set; }
        public decimal WinningBid { get; set; }
        public DateTime WonAt { get; set; } = DateTime.UtcNow;
        public bool IsPaid { get; set; } = false;
    }
}
