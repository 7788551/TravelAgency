using System.ComponentModel.DataAnnotations;

namespace TravelAgency.DTOs.Requests
{
    public class CreateStaffRequest
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}