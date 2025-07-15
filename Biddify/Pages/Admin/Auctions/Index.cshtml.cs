using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using Service.Impl;
using Biddify.Attributes;

namespace Biddify.Pages.Admin.Auctions
{
    [AdminAuthorization]
    public class IndexModel : PageModel
    {
        private readonly IAuctionProductService auctionProductService;

        public IndexModel(IAuctionProductService auctionProductService)
        {
            this.auctionProductService = auctionProductService;
            AuctionItems = new List<AuctionProductEntity>(); // Initialize to empty list
        }

        public List<AuctionProductEntity> AuctionItems { get; set; }
        public int TotalItems { get; set; }

        public async Task OnGetAsync()
        {
            var result = await auctionProductService.GetAuctionProductsAsync();
            AuctionItems = result.Items.ToList();
            TotalItems = result.TotalCount;
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var result = await auctionProductService.DeleteAuctionProductAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Auction deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete auction.";
            }

            return RedirectToPage();
        }
    }
}
