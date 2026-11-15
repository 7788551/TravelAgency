using System.ComponentModel.DataAnnotations;

namespace TravelAgency.DTOs.Requests
{
    public class UpdateEnquiryStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}