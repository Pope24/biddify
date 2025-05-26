using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MessageEntity
    {
        public Guid Id { get; set; }

        public Guid SenderId { get; set; }
        public UserEntity Sender { get; set; }
        public Guid ReceiverId { get; set; }
        public UserEntity Receiver { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
