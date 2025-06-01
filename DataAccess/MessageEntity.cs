using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MessageEntity
    {
        public string Id { get; set; }

        public string SenderId { get; set; }
        public UserEntity Sender { get; set; }
        public string ReceiverId { get; set; }
        public UserEntity Receiver { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
