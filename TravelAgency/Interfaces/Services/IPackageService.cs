using TravelAgency.DTOs.Requests;
using TravelAgency.Models;

namespace TravelAgency.Interfaces.Services
{
    public interface IPackageService
    {
        Task<List<Package>> GetAllAsync();

        Task<Package?> GetByIdAsync(int id);

        Task<int> CreateAsync(PackageRequest request);

        Task<bool> UpdateAsync(
            int id, PackageRequest request);

        Task<bool> DeleteAsync(int id);
    }
}