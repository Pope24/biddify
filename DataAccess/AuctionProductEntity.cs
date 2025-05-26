using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AuctionProductEntity
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public UserEntity Seller { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal StartPrice { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public EAuctionStatus Status { get; set; } = EAuctionStatus.PendingApproval;

        public ICollection<BidEntity> Bids { get; set; }
    }
}
