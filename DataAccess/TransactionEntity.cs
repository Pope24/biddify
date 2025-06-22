using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TransactionEntity
    {
        public string Id { get; set; }
        public string TransactionCode { get; set; }
        public string UserId { get; set; }
        public UserEntity User { get; set; }

        public ETransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public ETransactionStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
