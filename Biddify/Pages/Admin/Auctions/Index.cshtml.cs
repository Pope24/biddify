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
        }

        public List<AuctionProductEntity> AuctionItems { get; set; }

        public async Task OnGetAsync()
        {
            AuctionItems =
                (List<AuctionProductEntity>)await auctionProductService.GetAuctionProductsAsync();
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
