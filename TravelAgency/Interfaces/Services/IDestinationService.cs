using TravelAgency.DTOs.Requests;
using TravelAgency.Models;

namespace TravelAgency.Interfaces.Services
{
    public interface IDestinationService
    {
        Task<List<Destination>> GetAllAsync();

        Task<Destination?> GetByIdAsync(int id);

        Task<int> CreateAsync(DestinationRequest request);

        Task<bool> UpdateAsync(
            int id, DestinationRequest request);

        Task<bool> DeleteAsync(int id);
    }
}