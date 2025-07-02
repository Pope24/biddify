using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace Biddify.Pages.Auction;

public class AuctionModel : PageModel
{
    private readonly IAuctionProductService auctionProductService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public AuctionModel(IAuctionProductService auctionProductService, IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        this.auctionProductService = auctionProductService;
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    public List<AuctionProductEntity> AuctionItems { get; set; }

    public async Task OnGetAsync()
    {
        AuctionItems =
            (List<AuctionProductEntity>)await auctionProductService.GetAuctionProductsAsync();
    }

    public IActionResult OnPostSubmitBid(int ItemId)
    {
        // TODO: xử lý submit bid
        return RedirectToPage();
    }
    public async Task<IActionResult> OnGetVoiceSearchAsync(string transcript)
    {
        var products = await auctionProductService.GetAuctionProductsAsync();
        var nameProducts = products.Select(p => p.Title).ToList();

        var prompt = $@"
                        Bạn là trợ lý tìm kiếm sản phẩm.
                        Tôi có danh sách sản phẩm bằng tiếng Anh: {string.Join(", ", nameProducts)}.
                        Người dùng nói bằng tiếng Việt: ""{ transcript}"".
                        Hãy hiểu nghĩa câu nói của người dùng, so khớp với danh sách tôi đưa ra và trả về các sản phẩm phù hợp nhất.
                        Chỉ trả lời bằng JSON array chứa tên sản phẩm từ danh sách, ví dụ: [""Samsung Galaxy S23"", ""iPhone 15""].
                        Không thêm giải thích, không thêm sản phẩm ngoài danh sách.";
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _config["OpenAI:ApiKey"]);

        var requestBody = new
        {
            model = "mistralai/mistral-7b-instruct",
            messages = new[] { new { role = "user", content = prompt } },
            temperature = 0.2
        };

        var response = await client.PostAsync(
            "https://openrouter.ai/api/v1/chat/completions",
            new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return new JsonResult(new { error = responseContent });
        }

        var jsonDoc = JsonDocument.Parse(responseContent);
        var answer = jsonDoc.RootElement
                            .GetProperty("choices")[0]
                            .GetProperty("message")
                            .GetProperty("content")
                            .GetString() ?? "";
        List<string> productsRes = new List<string>();
        productsRes = JsonSerializer.Deserialize<List<string>>(answer);
        AuctionItems = products.Where(p => productsRes.Contains(p.Title)).ToList();
        return Page();
    }
}
