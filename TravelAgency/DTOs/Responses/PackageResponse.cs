namespace TravelAgency.DTOs.Responses
{
    public class PackageResponse
    {
        public int PackageId { get; set; }

        public string PackageName { get; set; } = string.Empty;

        public int DestinationId { get; set; }

        public string? DestinationName { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}