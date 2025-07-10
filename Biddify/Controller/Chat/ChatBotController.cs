using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Impl;

[ApiController]
[Route("api/chatbot")]
public class ChatBotController : ControllerBase
{
    private readonly AIService _aiService;

    public ChatBotController(AIService aiService)
    {
        _aiService = aiService;
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }

    public class ChatResponse
    {
        public object Response { get; set; } // để trả List<string> hoặc string
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChatRequest request)
    {
        var msg = request?.Message?.Trim();
        if (string.IsNullOrWhiteSpace(msg))
            return BadRequest("Message is required.");

        var faqService = new FAQService();
        var faq = faqService.GetFAQ();

        if (msg == "faq_titles")
        {
            var titles = new List<string>();
            if (faq.AccountAndSecurity?.Any() == true) titles.Add("AccountAndSecurity");
            if (faq.BiddingProcess?.Any() == true) titles.Add("BiddingProcess");
            if (faq.SellingItems?.Any() == true) titles.Add("SellingItems");
            if (faq.Payments?.Any() == true) titles.Add("Payments");
            if (faq.ShippingAndDelivery?.Any() == true) titles.Add("ShippingAndDelivery");

            return Ok(new ChatResponse { Response = titles });
        }

        if (msg.StartsWith("faq_get:"))
        {
            var group = msg.Substring(8);
            var list = group switch
            {
                "AccountAndSecurity" => faq.AccountAndSecurity,
                "BiddingProcess" => faq.BiddingProcess,
                "SellingItems" => faq.SellingItems,
                "Payments" => faq.Payments,
                "ShippingAndDelivery" => faq.ShippingAndDelivery,
                _ => new List<FAQItem>()
            };
            return Ok(new ChatResponse { Response = list });
        }

        // Câu hỏi cụ thể
        var allFaqs = faq.AccountAndSecurity
            .Concat(faq.BiddingProcess)
            .Concat(faq.SellingItems)
            .Concat(faq.Payments)
            .Concat(faq.ShippingAndDelivery)
            .ToList();

        var matched = allFaqs.FirstOrDefault(x => x.Question == msg);
        if (matched != null)
            return Ok(new ChatResponse { Response = matched.Answer });

        // Không tìm thấy → dùng AI
        var aiResponse = await _aiService.ProcessUserInputAsync(msg);
        return Ok(new ChatResponse { Response = aiResponse });
    }
}
