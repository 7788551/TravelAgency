using Microsoft.AspNetCore.Mvc;
using TravelAgency.DTOs.Requests;
using TravelAgency.Interfaces.Services;
using TravelAgency.Security;

namespace TravelAgency.ApiControllers
{
    [Route("api/packages")]
    [ApiController]
    public class PackageApiController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageApiController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        // GET: api/packages
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var packages = await _packageService.GetAllAsync();

            return Ok(packages);
        }

        // GET: api/packages/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var package = await _packageService.GetByIdAsync(id);

            if (package == null)
            {
                return NotFound(new
                {
                    message = "Package not found."
                });
            }

            return Ok(package);
        }

        // POST: api/packages
        [HttpPost]
        [SessionAuthorize("Admin", "Staff")]
        public async Task<IActionResult> Create(PackageRequest request)
        {
            try
            {
                int packageId =
                    await _packageService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = packageId },
                    new
                    {
                        packageId,
                        message = "Package created successfully."
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/packages/1
        [HttpPut("{id:int}")]
        [SessionAuthorize("Admin", "Staff")]
        public async Task<IActionResult> Update(
            int id,
            PackageRequest request)
        {
            try
            {
                bool updated =
                    await _packageService.UpdateAsync(id, request);

                if (!updated)
                {
                    return NotFound(new
                    {
                        message = "Package not found."
                    });
                }

                return Ok(new
                {
                    message = "Package updated successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE: api/packages/1
        [HttpDelete("{id:int}")]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool deleted =
                    await _packageService.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound(new
                    {
                        message = "Package not found."
                    });
                }

                return Ok(new
                {
                    message = "Package deleted successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}