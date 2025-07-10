
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

        public AIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ProcessUserInputAsync(string userInput)
        {
            var requestBody = new
            {
                model = "tngtech/deepseek-r1t2-chimera:free", 
                messages = new[]
                {
                    new { role = "system", content = "Bạn là trợ lý thông minh cho đấu giá trực tuyến." },
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