using System.ComponentModel.DataAnnotations;
using DataAccess;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using Domain.Models;

namespace Biddify.Pages.Admin.Auctions.Create
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAuctionProductService _auctionProductService;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<UserEntity> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IAuctionProductService auctionProductService,
            IWebHostEnvironment environment,
            UserManager<UserEntity> userManager,
            ILogger<IndexModel> logger
        )
        {
            _auctionProductService = auctionProductService;
            _environment = environment;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public CreateAuctionViewModel AuctionInput { get; set; } = new();

        [BindProperty]
        [Required(ErrorMessage = "At least one image is required")]
        public List<IFormFile> Images { get; set; } = new();

        public void OnGet()
        {
            // Set default values
            AuctionInput = new CreateAuctionViewModel
            {
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddDays(7)
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync called. Attempting to create a new auction.");

            if (AuctionInput.MinBidPrice >= AuctionInput.StartPrice)
            {
                _logger.LogWarning("Price validation failed: Min bid price is greater than or equal to start price. MinBidPrice: {MinBidPrice}, StartPrice: {StartPrice}", AuctionInput.MinBidPrice, AuctionInput.StartPrice);
                ModelState.AddModelError("AuctionInput.MinBidPrice", "Minimum bid amount must be less than start price");
            }
            if (AuctionInput.StartTime <= DateTime.Now)
            {
                _logger.LogWarning("Date validation failed: Start time is in the past. StartTime: {StartTime}", AuctionInput.StartTime);
                ModelState.AddModelError("AuctionInput.StartTime", "Start time must be in the future");
            }

            if (AuctionInput.EndTime <= AuctionInput.StartTime)
            {
                _logger.LogWarning("Date validation failed: End time is before start time. StartTime: {StartTime}, EndTime: {EndTime}", AuctionInput.StartTime, AuctionInput.EndTime);
                ModelState.AddModelError("AuctionInput.EndTime", "End time must be after start time");
            }

            // Basic validation
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Returning Page.");
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Any())
                    {
                        _logger.LogWarning($"ModelState Error in {state.Key}: {state.Value.Errors.First().ErrorMessage}");
                    }
                }
                return Page();
            }

            // Validate images
            if (Images == null || !Images.Any())
            {
                _logger.LogWarning("Image validation failed: No images provided.");
                ModelState.AddModelError("Images", "At least one image is required");
                return Page();
            }
            _logger.LogInformation("{ImageCount} images received.", Images.Count);

            try
            {
                _logger.LogInformation("Model is valid. Proceeding with auction creation.");
                // Get current user
                _logger.LogInformation("Getting current user.");
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogWarning("User is not authenticated. Redirecting to login page.");
                    return RedirectToPage("/Authen/Login");
                }
                _logger.LogInformation("User '{UserName}' found with ID: {UserId}", user.UserName, user.Id);

                // Map ViewModel to Entity
                var auctionProduct = new AuctionProductEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = AuctionInput.Title,
                    Description = AuctionInput.Description,
                    CategoryProduct = AuctionInput.CategoryProduct.Value,
                    StartPrice = AuctionInput.StartPrice,
                    MinBidPrice = AuctionInput.MinBidPrice,
                    StartTime = AuctionInput.StartTime.ToUniversalTime(),
                    EndTime = AuctionInput.EndTime.ToUniversalTime(),
                    SellerId = user.Id,
                    Status = EAuctionStatus.PendingApproval,
                    ImageUrls = new List<string>()
                };

                _logger.LogInformation("Assigned new Auction ID: {AuctionId}", auctionProduct.Id);

                // Handle image uploads
                if (Images != null && Images.Any())
                {
                    _logger.LogInformation("Starting image upload process.");
                    var uploadPath = Path.Combine(
                        _environment.WebRootPath,
                        "uploads",
                        "auctions",
                        auctionProduct.Id
                    );
                    Directory.CreateDirectory(uploadPath);
                    _logger.LogInformation("Created directory for auction images at: {UploadPath}", uploadPath);

                    foreach (var image in Images)
                    {
                        _logger.LogInformation("Processing image: {FileName}, Size: {FileSize}, ContentType: {ContentType}", image.FileName, image.Length, image.ContentType);
                        // Validate image
                        if (!image.ContentType.StartsWith("image/"))
                        {
                            _logger.LogWarning("File '{FileName}' is not a valid image. ContentType: {ContentType}", image.FileName, image.ContentType);
                            ModelState.AddModelError(
                                "Images",
                                $"File '{image.FileName}' is not a valid image"
                            );
                            return Page();
                        }

                        if (image.Length > 10 * 1024 * 1024) // 10MB limit
                        {
                            _logger.LogWarning("File '{FileName}' exceeds 10MB limit. Size: {FileSize}", image.FileName, image.Length);
                            ModelState.AddModelError(
                                "Images",
                                $"File '{image.FileName}' exceeds 10MB limit"
                            );
                            return Page();
                        }

                        if (image.Length > 0)
                        {
                            var fileName =
                                Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                            var filePath = Path.Combine(uploadPath, fileName);

                            _logger.LogInformation("Saving image to: {FilePath}", filePath);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }
                            _logger.LogInformation("Image saved successfully.");

                            auctionProduct.ImageUrls.Add(
                                $"/uploads/auctions/{auctionProduct.Id}/{fileName}"
                            );
                        }
                    }
                }

                // Save to database
                _logger.LogInformation("Attempting to save auction product to the database. Auction ID: {AuctionId}", auctionProduct.Id);
                var result = await _auctionProductService.AddAuctionProductAsync(auctionProduct);

                if (result)
                {
                    _logger.LogInformation("Auction created successfully! Auction ID: {AuctionId}. Redirecting to index.", auctionProduct.Id);
                    TempData["SuccessMessage"] = "Auction created successfully!";
                    return RedirectToPage("/Admin/Auctions/Index");
                }
                else
                {
                    _logger.LogError("Failed to save auction to database. Auction ID: {AuctionId}", auctionProduct.Id);
                    // If save fails, cleanup uploaded images
                    var uploadPath = Path.Combine(
                        _environment.WebRootPath,
                        "uploads",
                        "auctions",
                        auctionProduct.Id
                    );
                    if (Directory.Exists(uploadPath))
                    {
                        _logger.LogInformation("Cleaning up uploaded images for failed auction creation. Path: {UploadPath}", uploadPath);
                        Directory.Delete(uploadPath, true);
                    }

                    ModelState.AddModelError("", "Failed to create auction. Please try again.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                var auctionId = "not-yet-created";
              
                _logger.LogError(ex, "An unhandled exception occurred during auction creation.");

                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return Page();
            }
        }
    }
}
