using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Models
{
    public class CreateAuctionViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Title must be between 10 and 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(5000, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 5000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public ECategoryProduct? CategoryProduct { get; set; }

        [Required(ErrorMessage = "Start price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Start price must be greater than 0")]
        public decimal StartPrice { get; set; }

        [Required(ErrorMessage = "Minimum bid amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Minimum bid amount must be greater than 0")]
        public decimal MinBidPrice { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; set; } = DateTime.Now.AddHours(1);

        [Required(ErrorMessage = "End time is required")]
        public DateTime EndTime { get; set; } = DateTime.Now.AddDays(7);
    }
} 