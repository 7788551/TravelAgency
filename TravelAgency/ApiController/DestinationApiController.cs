using Microsoft.AspNetCore.Mvc;
using TravelAgency.DTOs.Requests;
using TravelAgency.Interfaces.Services;
using TravelAgency.Security;

namespace TravelAgency.ApiControllers
{
    [Route("api/destinations")]
    [ApiController]
    public class DestinationApiController : ControllerBase
    {
        private readonly IDestinationService _destinationService;

        public DestinationApiController(
            IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        // GET: api/destinations
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var destinations = await _destinationService.GetAllAsync();

            return Ok(destinations);
        }

        // GET: api/destinations/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var destination = await _destinationService.GetByIdAsync(id);

            if (destination == null)
            {
                return NotFound(new
                {
                    message = "Destination not found."
                });
            }

            return Ok(destination);
        }

        // POST: api/destinations
        [HttpPost]
        [SessionAuthorize("Admin", "Staff")]
        public async Task<IActionResult> Create(DestinationRequest request)
        {
            try
            {
                int destinationId =
                    await _destinationService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = destinationId },
                    new
                    {
                        destinationId,
                        message = "Destination created successfully."
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/destinations/1
        [HttpPut("{id:int}")]
        [SessionAuthorize("Admin", "Staff")]
        public async Task<IActionResult> Update(
            int id,
            DestinationRequest request)
        {
            try
            {
                bool updated =
                    await _destinationService.UpdateAsync(id, request);

                if (!updated)
                {
                    return NotFound(new
                    {
                        message = "Destination not found."
                    });
                }

                return Ok(new
                {
                    message = "Destination updated successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/destinations/1
        [HttpDelete("{id:int}")]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool deleted =
                    await _destinationService.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound(new
                    {
                        message = "Destination not found."
                    });
                }

                return Ok(new
                {
                    message = "Destination deleted successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}