using System.ComponentModel.DataAnnotations;

namespace TravelAgency.DTOs.Requests
{
    public class EnquiryRequest
    {
        [Required(ErrorMessage = "Package is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a valid package.")]
        public int PackageId { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters.")]
        public string Message { get; set; } = string.Empty;
    }
}