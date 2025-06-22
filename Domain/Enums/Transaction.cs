using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum ETransactionType
    {
        Deposit,
        Withdraw,
        Payment,    
        ReceivePayment 
    }

    public enum ETransactionStatus
    {
        Pending,
        Success,
        Failed,
        Cancelled
    }

}
