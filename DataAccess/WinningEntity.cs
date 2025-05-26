using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class WinningEntity
    {
        public Guid Id { get; set; }

        public Guid AuctionProductId { get; set; }
        public AuctionProductEntity AuctionProduct { get; set; }
        public Guid WinnerId { get; set; }
        public UserEntity Winner { get; set; }
        public decimal WinningBid { get; set; }
        public DateTime WonAt { get; set; } = DateTime.UtcNow;
        public bool IsPaid { get; set; } = false;
    }
}
