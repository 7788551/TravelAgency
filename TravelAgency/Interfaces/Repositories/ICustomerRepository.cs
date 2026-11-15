using TravelAgency.Models;

namespace TravelAgency.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<int> CreateEnquiryAsync(CustomerEnquiry enquiry);

        Task<List<CustomerEnquiry>> GetMyEnquiriesAsync(int userId);

        Task<List<CustomerEnquiry>> GetAllEnquiriesAsync();

        Task<bool> UpdateEnquiryStatusAsync(
            int enquiryId,
            string status);
    }
}