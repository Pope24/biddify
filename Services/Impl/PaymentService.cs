using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
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

        public PaymentService(IConfiguration config, PayOS payOS)
        {
            _config = config;
            _payOS = payOS;
        }

        public async Task<string> CreatePaymentLinkOS(decimal amount, string desc, string returnUrl)
        {
            int amountInVND = (int)(amount*26018);
            var items = new List<ItemData>
            {
                new ItemData("Deposit to account", 1, amountInVND)
            };
            var paymentData = new PaymentData(
                orderCode: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                amount: amountInVND,
                description: desc,
                items: items,
                cancelUrl: returnUrl,
                returnUrl: returnUrl
            );

            var result = await _payOS.createPaymentLink(paymentData);

            return result.checkoutUrl;
        }
    }

}
