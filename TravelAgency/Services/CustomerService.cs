using TravelAgency.DTOs.Requests;
using TravelAgency.Interfaces.Repositories;
using TravelAgency.Interfaces.Services;
using TravelAgency.Models;

namespace TravelAgency.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IPackageRepository _packageRepository;

        public CustomerService(
            ICustomerRepository repository,
            IPackageRepository packageRepository)
        {
            _repository = repository;
            _packageRepository = packageRepository;
        }

        // CREATE CUSTOMER ENQUIRY
        public async Task<int> CreateEnquiryAsync(
            int userId, EnquiryRequest request)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid customer.");

            if (request.PackageId <= 0)
                throw new ArgumentException("Invalid package.");

            if (string.IsNullOrWhiteSpace(request.Message))
                throw new ArgumentException(
                    "Enquiry message is required.");

            // Check whether package exists
            var package =
                await _packageRepository.GetByIdAsync(
                    request.PackageId);

            if (package == null)
            {
                throw new InvalidOperationException(
                    "Selected package does not exist.");
            }

            var enquiry = new CustomerEnquiry
            {
                UserId = userId,
                PackageId = request.PackageId,
                Message = request.Message.Trim(),
                Status = "Pending"
            };

            return await _repository.CreateEnquiryAsync(enquiry);
        }

        // GET CUSTOMER'S OWN ENQUIRIES
        public Task<List<CustomerEnquiry>> GetMyEnquiriesAsync(
            int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid customer.");

            return _repository.GetMyEnquiriesAsync(userId);
        }

        // GET ALL ENQUIRIES
        public Task<List<CustomerEnquiry>> GetAllEnquiriesAsync()
        {
            return _repository.GetAllEnquiriesAsync();
        }

        // UPDATE ENQUIRY STATUS
        public async Task<bool> UpdateEnquiryStatusAsync(
            int enquiryId, string status)
        {
            if (enquiryId <= 0)
                throw new ArgumentException("Invalid enquiry ID.");

            string[] allowedStatuses =
            {
                "Pending",
                "Contacted",
                "Resolved",
                "Cancelled"
            };

            if (!allowedStatuses.Contains(
                status,
                StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Invalid enquiry status.");
            }

            // Normalize status to the database values
            string normalizedStatus =
                allowedStatuses.First(
                    x => x.Equals(
                        status,
                        StringComparison.OrdinalIgnoreCase));

            return await _repository.UpdateEnquiryStatusAsync(
                enquiryId,
                normalizedStatus);
        }
    }
}