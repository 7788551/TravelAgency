using TravelAgency.DTOs.Requests;
using TravelAgency.Models;

namespace TravelAgency.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<int> CreateEnquiryAsync(
            int userId, EnquiryRequest request);

        Task<List<CustomerEnquiry>> GetMyEnquiriesAsync(
            int userId);

        Task<List<CustomerEnquiry>> GetAllEnquiriesAsync();

        Task<bool> UpdateEnquiryStatusAsync(
            int enquiryId, string status);
    }
}