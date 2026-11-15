using System.ComponentModel.DataAnnotations;

namespace TravelAgency.DTOs.Requests
{
    public class PackageRequest
    {
        [Required(ErrorMessage = "Package name is required.")]
        [StringLength(150)]
        public string PackageName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Select a valid destination.")]
        public int DestinationId { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0.01, 9999999999, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days.")]
        public int DurationDays { get; set; }
    }
}