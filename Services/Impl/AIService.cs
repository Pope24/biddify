
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Impl
{
    public class AIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "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"; 
        private readonly string _baseUrl = "https://openrouter.ai/api/v1/chat/completions";
        private readonly IAuctionProductService auctionProductService;
        public AIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private bool IsProductInquiry(string input)
        {
            input = input.ToLowerInvariant();

            return input.Contains("món gì")
                || input.Contains("đấu giá gì")
                || input.Contains("sản phẩm")
                || input.Contains("món đấu giá")
                || input.Contains("có gì bán")
                || input.Contains("đang bán")
                || input.Contains("đang đấu giá")
                || input.Contains("có gì không");
        }

        public async Task<string> ProcessUserInputAsync(string userInput)
        {
            //if (IsProductInquiry(userInput))
            //{
            //    var products = await auctionProductService.GetAuctionProductsAsync();

            //    if (products != null && products.Any())
            //    {
            //        var productDescriptions = string.Join("\n", products.Select(p =>
            //            $"- {p.Title} (Giá khởi điểm: {p.StartPrice:N0} VND, Thời gian: {p.StartTime:dd/MM/yyyy HH:mm} - {p.EndTime:dd/MM/yyyy HH:mm})"));

            //        userInput += $"\n\nDanh sách các sản phẩm đang đấu giá:\n{productDescriptions}";
            //    }
            //    else
            //    {
            //        userInput += "\n\nHiện tại không có sản phẩm nào đang được đấu giá.";
            //    }
            //}

            var requestBody = new
            {
                model = "tngtech/deepseek-r1t2-chimera:free",
                messages = new[]
                {
            new { role = "system", content = "Bạn là AI hỗ trợ cho đấu giá trực tuyến." },
            new { role = "user", content = userInput }
        }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            request.Headers.Add("HTTP-Referer", "https://localhost:32769/");
            request.Headers.Add("X-Title", "Biddify ChatBot");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return $"[Lỗi OpenRouter: {response.StatusCode}] {error}";
            }

            var json = await response.Content.ReadAsStringAsync();
            return ExtractResultFrom(json);
        }

        private string ExtractResultFrom(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var message = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return message ?? "Không có phản hồi.";
            }
            catch
            {
                return "Không thể phân tích phản hồi từ mô hình.";
            }
        }
    }
}