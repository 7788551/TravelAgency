using TravelAgency.DTOs.Requests;
using TravelAgency.Interfaces.Repositories;
using TravelAgency.Interfaces.Services;
using TravelAgency.Models;

namespace TravelAgency.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _repository;
        private readonly IDestinationRepository _destinationRepository;

        public PackageService(
            IPackageRepository repository,
            IDestinationRepository destinationRepository)
        {
            _repository = repository;
            _destinationRepository = destinationRepository;
        }

        // GET ALL
        public Task<List<Package>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        // GET BY ID
        public Task<Package?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid package ID.");

            return _repository.GetByIdAsync(id);
        }

        // CREATE
        public async Task<int> CreateAsync(PackageRequest request)
        {
            ValidatePackage(request);

            // Check whether destination exists
            var destination =
                await _destinationRepository.GetByIdAsync(
                    request.DestinationId);

            if (destination == null)
            {
                throw new InvalidOperationException(
                    "Selected destination does not exist.");
            }

            var package = new Package
            {
                PackageName = request.PackageName.Trim(),
                DestinationId = request.DestinationId,
                Description = request.Description?.Trim(),
                Price = request.Price,
                DurationDays = request.DurationDays
            };

            return await _repository.CreateAsync(package);
        }

        // UPDATE
        public async Task<bool> UpdateAsync(
            int id, PackageRequest request)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid package ID.");

            ValidatePackage(request);

            var existing =
                await _repository.GetByIdAsync(id);

            if (existing == null)
                return false;

            var destination =
                await _destinationRepository.GetByIdAsync(
                    request.DestinationId);

            if (destination == null)
            {
                throw new InvalidOperationException(
                    "Selected destination does not exist.");
            }

            existing.PackageName = request.PackageName.Trim();
            existing.DestinationId = request.DestinationId;
            existing.Description = request.Description?.Trim();
            existing.Price = request.Price;
            existing.DurationDays = request.DurationDays;

            return await _repository.UpdateAsync(id, existing);
        }

        // DELETE
        public Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid package ID.");

            return _repository.DeleteAsync(id);
        }

        // COMMON VALIDATION
        private static void ValidatePackage(PackageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PackageName))
            {
                throw new ArgumentException(
                    "Package name is required.");
            }

            if (request.DestinationId <= 0)
            {
                throw new ArgumentException(
                    "Please select a valid destination.");
            }

            if (request.Price <= 0)
            {
                throw new ArgumentException(
                    "Package price must be greater than zero.");
            }

            if (request.DurationDays <= 0)
            {
                throw new ArgumentException(
                    "Package duration must be greater than zero.");
            }
        }
    }
}