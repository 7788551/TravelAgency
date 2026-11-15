namespace TravelAgency.Models
{
    public class Dashboard
    {
        public int TotalUsers { get; set; }

        public int TotalStaff { get; set; }

        public int TotalCustomers { get; set; }

        public int TotalDestinations { get; set; }

        public int TotalPackages { get; set; }

        public int TotalEnquiries { get; set; }

        public int PendingEnquiries { get; set; }

        public int ResolvedEnquiries { get; set; }
    }
}