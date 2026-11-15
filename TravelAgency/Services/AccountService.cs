using TravelAgency.DTOs.Requests;
using TravelAgency.Interfaces.Repositories;
using TravelAgency.Interfaces.Services;
using TravelAgency.Models;
using TravelAgency.Security;

namespace TravelAgency.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // CUSTOMER REGISTRATION
        public async Task<int> RegisterAsync(
            RegisterRequest request)
        {
            string email = request.Email.Trim();

            // Check duplicate email
            var existingUser =
                await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "Email is already registered.");
            }

            // Hash password
            string passwordHash =
                PasswordHelper.HashPassword(request.Password);

            var user = new User
            {
                FullName = request.FullName.Trim(),
                Email = email,
                PasswordHash = passwordHash
            };

            // Repository always assigns Customer role
            return await _userRepository.RegisterCustomerAsync(user);
        }

        // USER LOGIN
        public async Task<User?> LoginAsync(LoginRequest request)
        {
            string email = request.Email.Trim();

            var user =
                await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return null;

            // Verify BCrypt password
            bool isValid =
                PasswordHelper.VerifyPassword(
                    request.Password,
                    user.PasswordHash);

            if (!isValid)
                return null;

            return user;
        }

        // ADMIN CREATES STAFF
        public async Task<int> CreateStaffAsync(
            CreateStaffRequest request)
        {
            string email = request.Email.Trim();

            var existingUser =
                await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "Email is already registered.");
            }

            string passwordHash =
                PasswordHelper.HashPassword(request.Password);

            var staff = new User
            {
                FullName = request.FullName.Trim(),
                Email = email,
                PasswordHash = passwordHash
            };

            // Repository always assigns Staff role
            return await _userRepository.CreateStaffAsync(staff);
        }

        // GET ALL STAFF
        public Task<List<User>> GetStaffAsync()
        {
            return _userRepository.GetStaffAsync();
        }
    }
}