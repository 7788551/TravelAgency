using Microsoft.AspNetCore.Mvc;
using TravelAgency.DTOs.Requests;
using TravelAgency.DTOs.Responses;
using TravelAgency.Interfaces.Services;
using TravelAgency.Security;

namespace TravelAgency.ApiControllers
{
    [Route("api/staff")]
    [ApiController]
    [SessionAuthorize("Admin")]
    public class StaffApiController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public StaffApiController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // POST: api/staff
        [HttpPost]
        public async Task<IActionResult> CreateStaff(
            CreateStaffRequest request)
        {
            try
            {
                int staffId =
                    await _accountService.CreateStaffAsync(request);

                return StatusCode(201, new
                {
                    staffId,
                    message = "Staff account created successfully."
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

        // GET: api/staff
        [HttpGet]
        public async Task<IActionResult> GetAllStaff()
        {
            var staff = await _accountService.GetStaffAsync();

            // Return safe user information only.
            var response = staff.Select(user => new UserResponse
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                RoleName = user.RoleName
            }).ToList();

            return Ok(response);
        }
    }
}