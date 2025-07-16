using Common;
using DataAccess;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class PaymentService : IPaymentService
    {
        private readonly PayOS _payOS;
        private readonly IConfiguration _config;
        private readonly ITransactionRepository _transactionRepository;

        public PaymentService(IConfiguration config, PayOS payOS, ITransactionRepository transactionRepository)
        {
            _config = config;
            _payOS = payOS;
            _transactionRepository = transactionRepository;
        }

        public async Task<string> CreatePaymentLinkOS(string userId, string? auctionProductId, string type, decimal amount, string desc, string returnUrl)
        {
            var _orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            int amountInVND = (int)(amount * 26018);
            var entity = new TransactionEntity
            {
                Id = Guid.NewGuid().ToString(),
                TransactionCode = _orderCode.ToString(),
                UserId = userId,
                Type = EnumHelper.ParseDisplayString<ETransactionType>(type),
                Status = ETransactionStatus.Pending,
                Amount = amountInVND,
                CreatedAt = DateTime.UtcNow,
                Metadata = new TransactionMetadata()
                {
                    AuctionProductId = auctionProductId,
                }
            };
            await _transactionRepository.AddTransactionAsync(entity);
            var items = new List<ItemData>
            {
                new ItemData($"{type} {auctionProductId}", 1, amountInVND)
            };

            var paymentData = new PaymentData(
                orderCode: _orderCode,
                amount: amountInVND,
                description: $"{type}|{auctionProductId}",
                items: items,
                cancelUrl: returnUrl,
                returnUrl: returnUrl
            );

            var result = await _payOS.createPaymentLink(paymentData);

            return result.checkoutUrl;
        }
    }

}
