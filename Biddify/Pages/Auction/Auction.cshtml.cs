using DataAccess;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        AuctionItems = new List<AuctionProductEntity>(); // Initialize to empty list
    }

    public List<AuctionProductEntity> AuctionItems { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public EAuctionStatus? StatusFilter { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public ECategoryProduct? CategoryFilter { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string TimeFilter { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    
    public int PageSize { get; set; } = 9;
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    
    public SelectList StatusSelectList { get; set; }
    public SelectList CategorySelectList { get; set; }
    public SelectList TimeSelectList { get; set; }

    public async Task OnGetAsync()
    {
        // Ensure current page is at least 1
        if (CurrentPage < 1)
        {
            CurrentPage = 1;
        }
        
        // Initialize the select lists
        StatusSelectList = new SelectList(
            Enum.GetValues(typeof(EAuctionStatus))
                .Cast<EAuctionStatus>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = GetStatusDisplayName(s)
                }),
            "Value", "Text");
        
        CategorySelectList = new SelectList(
            Enum.GetValues(typeof(ECategoryProduct))
                .Cast<ECategoryProduct>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = GetCategoryDisplayName(c)
                }),
            "Value", "Text");
        
        TimeSelectList = new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "All" },
            new SelectListItem { Value = "current", Text = "Current Auctions" },
            new SelectListItem { Value = "upcoming", Text = "Upcoming Auctions" }
        }, "Value", "Text");

        // Apply filters
        bool? isCurrent = null;
        
        if (TimeFilter == "current")
        {
            isCurrent = true;
        }
        else if (TimeFilter == "upcoming")
        {
            isCurrent = false;
        }

        // Get auction items with filters and pagination
        var result = await auctionProductService.GetAuctionProductsAsync(
            SearchTerm ?? "", 
            StatusFilter, 
            CategoryFilter, 
            isCurrent,
            CurrentPage,
            PageSize);
            
        AuctionItems = result.Items.ToList();
        TotalItems = result.TotalCount;
    }

    public IActionResult OnPostSubmitBid(int ItemId)
    {
        // TODO: xử lý submit bid
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnGetVoiceSearchAsync(string transcript)
    {
        var result = await auctionProductService.GetAuctionProductsAsync();
        var nameProducts = result.Items.Select(p => p.Title).ToList();

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
        AuctionItems = result.Items.Where(p => productsRes.Contains(p.Title)).ToList();
        TotalItems = AuctionItems.Count;
        return Page();
    }
    
    private string GetStatusDisplayName(EAuctionStatus status)
    {
        return status switch
        {
            EAuctionStatus.PendingApproval => "Pending Approval",
            EAuctionStatus.Active => "Active",
            EAuctionStatus.Ended => "Ended",
            EAuctionStatus.Cancelled => "Cancelled",
            _ => status.ToString()
        };
    }
    
    private string GetCategoryDisplayName(ECategoryProduct category)
    {
        return category switch
        {
            ECategoryProduct.DiamondRing => "Diamond Ring",
            ECategoryProduct.Necklace => "Necklace",
            ECategoryProduct.Bracelet => "Bracelet",
            ECategoryProduct.Earring => "Earring",
            ECategoryProduct.LooseGemstone => "Loose Gemstone",
            ECategoryProduct.JewelryWatch => "Jewelry Watch",
            _ => category.ToString()
        };
    }
}
