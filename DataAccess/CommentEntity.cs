using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CommentEntity
    {
        public string Id { get; set; }

        public string AuctionProductId { get; set; }
        public AuctionProductEntity AuctionProduct { get; set; }

        public string UserId { get; set; }
        public UserEntity User { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
