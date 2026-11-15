using TravelAgency.Models;

namespace TravelAgency.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<int> RegisterCustomerAsync(User user);

        Task<int> CreateStaffAsync(User user);

        Task<User?> GetByEmailAsync(string email);

        Task<List<User>> GetStaffAsync();
    }
}