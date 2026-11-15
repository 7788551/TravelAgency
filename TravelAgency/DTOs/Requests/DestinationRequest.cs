using System.ComponentModel.DataAnnotations;

namespace TravelAgency.DTOs.Requests
{
    public class DestinationRequest
    {
        [Required(ErrorMessage = "Destination name is required.")]
        [StringLength(100)]
        public string DestinationName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}