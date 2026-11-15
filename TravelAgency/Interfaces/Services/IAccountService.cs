using TravelAgency.DTOs.Requests;
using TravelAgency.Models;

namespace TravelAgency.Interfaces.Services
{
    public interface IAccountService
    {
        Task<int> RegisterAsync(RegisterRequest request);

        Task<User?> LoginAsync(LoginRequest request);

        Task<int> CreateStaffAsync(CreateStaffRequest request);

        Task<List<User>> GetStaffAsync();
    }
}