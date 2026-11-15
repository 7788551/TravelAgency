using TravelAgency.Models;

namespace TravelAgency.Interfaces.Repositories
{
    public interface IPackageRepository
    {
        Task<List<Package>> GetAllAsync();

        Task<Package?> GetByIdAsync(int id);

        Task<int> CreateAsync(Package package);

        Task<bool> UpdateAsync(int id, Package package);

        Task<bool> DeleteAsync(int id);
    }
}