using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentLinkOS(string userId, string? auctionProductId, string status, decimal amount, string description, string returnUrl);
    }
}
