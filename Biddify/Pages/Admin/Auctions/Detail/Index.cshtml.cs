using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Threading.Tasks;

namespace Biddify.Pages.Admin.Auctions.Detail
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAuctionProductService _auctionProductService;

        public IndexModel(IAuctionProductService auctionProductService)
        {
            _auctionProductService = auctionProductService;
        }

        public AuctionProductEntity AuctionProduct { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Fetch the auction product including the seller information
            AuctionProduct = await _auctionProductService.GetAuctionProductByIdAsync(id);

            if (AuctionProduct == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
