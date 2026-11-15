using TravelAgency.Models;

namespace TravelAgency.Interfaces.Repositories
{
    public interface IDestinationRepository
    {
        Task<List<Destination>> GetAllAsync();

        Task<Destination?> GetByIdAsync(int id);

        Task<int> CreateAsync(Destination destination);

        Task<bool> UpdateAsync(int id, Destination destination);

        Task<bool> DeleteAsync(int id);
    }
}