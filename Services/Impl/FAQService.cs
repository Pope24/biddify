using Domain.Models;
using System.IO;
using System.Text.Json;

namespace Service.Impl
{
    public class FAQService
    {
        private readonly string _filePath;

        public FAQService()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Pages","Shared", "Chat", "Faq.json");
        }

        public FAQQuestion GetFAQ()
        {
            if (!File.Exists(_filePath))
                return new FAQQuestion();

            string json = File.ReadAllText(_filePath);

           
            var data = JsonSerializer.Deserialize<ChatMessageModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

     
            data.FAQQuestion ??= new FAQQuestion();
            data.FAQQuestion.AccountAndSecurity ??= new List<FAQItem>();
            data.FAQQuestion.BiddingProcess ??= new List<FAQItem>();
            data.FAQQuestion.SellingItems ??= new List<FAQItem>();
            data.FAQQuestion.Payments ??= new List<FAQItem>();
            data.FAQQuestion.ShippingAndDelivery ??= new List<FAQItem>();

            return data.FAQQuestion;
        }
    }
}
