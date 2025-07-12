using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DataAccess;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace Biddify.Pages.Admin.Auctions.Edit
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAuctionProductService _auctionProductService;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(
            IAuctionProductService auctionProductService,
            IWebHostEnvironment environment
        )
        {
            _auctionProductService = auctionProductService;
            _environment = environment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Title { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public ECategoryProduct CategoryProduct { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            public decimal StartPrice { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            public decimal MinBidPrice { get; set; }

            [Required]
            public DateTime StartTime { get; set; }

            [Required]
            public DateTime EndTime { get; set; }

            [Required]
            public EAuctionStatus Status { get; set; }

            public List<string> ExistingImageUrls { get; set; } = new();
        }

        [BindProperty]
        public List<IFormFile> NewImages { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            var auction = await _auctionProductService.GetAuctionProductByIdAsync(id);
            if (auction == null)
                return NotFound();

            Input = new InputModel
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                CategoryProduct = auction.CategoryProduct,
                StartPrice = auction.StartPrice,
                MinBidPrice = auction.MinBidPrice,
                StartTime = auction.StartTime.ToLocalTime(),
                EndTime = auction.EndTime.ToLocalTime(),
                Status = auction.Status,
                ExistingImageUrls = auction.ImageUrls ?? new List<string>(),
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var auctionToUpdate = await _auctionProductService.GetAuctionProductByIdAsync(Input.Id);
            if (auctionToUpdate == null)
            {
                return NotFound();
            }

            // Update properties
            auctionToUpdate.Title = Input.Title;
            auctionToUpdate.Description = Input.Description;
            auctionToUpdate.CategoryProduct = Input.CategoryProduct;
            auctionToUpdate.StartPrice = Input.StartPrice;
            auctionToUpdate.MinBidPrice = Input.MinBidPrice;
            auctionToUpdate.StartTime = Input.StartTime.ToUniversalTime();
            auctionToUpdate.EndTime = Input.EndTime.ToUniversalTime();
            auctionToUpdate.Status = Input.Status;

            // Handle image updates
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "auctions", Input.Id);
            Directory.CreateDirectory(uploadPath);

            // Process new images
            if (NewImages != null && NewImages.Any())
            {
                foreach (var image in NewImages)
                {
                    if (image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        var imageUrl = $"/uploads/auctions/{Input.Id}/{fileName}";
                        auctionToUpdate.ImageUrls.Add(imageUrl);
                    }
                }
            }

            // Update existing images
            var existingUrls = Request.Form["Input.ExistingImageUrls"].ToList();
            var imagesToRemove = auctionToUpdate.ImageUrls
                .Where(url => !existingUrls.Contains(url))
                .ToList();

            // Remove files that are no longer in the list
            foreach (var imageUrl in imagesToRemove)
            {
                var fileName = Path.GetFileName(imageUrl);
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", "auctions", Input.Id, fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Update the image URLs in the database
            auctionToUpdate.ImageUrls = existingUrls;

            await _auctionProductService.UpdateAuctionProductAsync(auctionToUpdate);

            TempData["SuccessMessage"] = "Auction updated successfully!";
            return RedirectToPage("/Admin/Auctions/Detail/Index", new { id = Input.Id });
        }
    }
}
