namespace TravelAgency.Models
{
    public class CustomerEnquiry
    {
        public int EnquiryId { get; set; }

        public int UserId { get; set; }

        public int PackageId { get; set; }

        public string Message { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; }

        // Additional display properties populated through SQL JOINs.
        public string CustomerName { get; set; } = string.Empty;

        public string PackageName { get; set; } = string.Empty;
    }
}