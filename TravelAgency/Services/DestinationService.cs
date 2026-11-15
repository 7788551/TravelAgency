using TravelAgency.DTOs.Requests;
using TravelAgency.Interfaces.Repositories;
using TravelAgency.Interfaces.Services;
using TravelAgency.Models;

namespace TravelAgency.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _repository;

        public DestinationService(
            IDestinationRepository repository)
        {
            _repository = repository;
        }

        // GET ALL
        public Task<List<Destination>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        // GET BY ID
        public Task<Destination?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid destination ID.");

            return _repository.GetByIdAsync(id);
        }

        // CREATE
        public Task<int> CreateAsync(DestinationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.DestinationName))
                throw new ArgumentException(
                    "Destination name is required.");

            if (string.IsNullOrWhiteSpace(request.Country))
                throw new ArgumentException(
                    "Country is required.");

            var destination = new Destination
            {
                DestinationName = request.DestinationName.Trim(),
                Country = request.Country.Trim(),
                Description = request.Description?.Trim()
            };

            return _repository.CreateAsync(destination);
        }

        // UPDATE
        public async Task<bool> UpdateAsync(
            int id, DestinationRequest request)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid destination ID.");

            if (string.IsNullOrWhiteSpace(request.DestinationName) ||
                string.IsNullOrWhiteSpace(request.Country))
            {
                throw new ArgumentException(
                    "Destination name and country are required.");
            }

            var existing =
                await _repository.GetByIdAsync(id);

            if (existing == null)
                return false;

            existing.DestinationName =
                request.DestinationName.Trim();

            existing.Country = request.Country.Trim();

            existing.Description = request.Description?.Trim();

            return await _repository.UpdateAsync(id, existing);
        }

        // DELETE
        public Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid destination ID.");

            return _repository.DeleteAsync(id);
        }
    }
}