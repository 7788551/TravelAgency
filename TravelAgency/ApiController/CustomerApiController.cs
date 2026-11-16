using Microsoft.AspNetCore.Mvc;
using TravelAgency.DTOs.Requests;
using TravelAgency.Interfaces.Services;
using TravelAgency.Security;

namespace TravelAgency.ApiControllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerApiController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // POST: api/customer/enquiries
        [HttpPost("enquiries")]
        [SessionAuthorize("Customer")]
        public async Task<IActionResult> CreateEnquiry(
            EnquiryRequest request)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    return Unauthorized(new
                    {
                        message = "Please log in first."
                    });
                }

                int enquiryId =
                    await _customerService.CreateEnquiryAsync(
                        userId.Value,
                        request);

                return Ok(new
                {
                    enquiryId,
                    message = "Enquiry submitted successfully."
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

        // GET: api/customer/my-enquiries
        [HttpGet("my-enquiries")]
        [SessionAuthorize("Customer")]
        public async Task<IActionResult> GetMyEnquiries()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized(new
                {
                    message = "Please log in first."
                });
            }

            var enquiries =
                await _customerService.GetMyEnquiriesAsync(userId.Value);

            return Ok(enquiries);
        }

        // GET: api/customer/enquiries
        [HttpGet("enquiries")]
        [SessionAuthorize("Admin", "Staff")]
        public async Task<IActionResult> GetAllEnquiries()
        {
            var enquiries =
                await _customerService.GetAllEnquiriesAsync();

            return Ok(enquiries);
        }

        // PUT: api/customer/enquiries/1/status
        [HttpPut("enquiries/{id:int}/status")]
        [SessionAuthorize("Admin", "Staff")]
        public async Task<IActionResult> UpdateEnquiryStatus(
            int id,
            UpdateEnquiryStatusRequest request)
        {
            try
            {
                bool updated =
                    await _customerService.UpdateEnquiryStatusAsync(
                        id,
                        request.Status);

                if (!updated)
                {
                    return NotFound(new
                    {
                        message = "Enquiry not found."
                    });
                }

                return Ok(new
                {
                    message = "Enquiry status updated successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}