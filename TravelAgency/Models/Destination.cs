namespace TravelAgency.Models
{
    public class Destination
    {
        public int DestinationId { get; set; }

        public string DestinationName { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}