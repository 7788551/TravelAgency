using Microsoft.AspNetCore.Mvc;
using TravelAgency.DTOs.Requests;
using TravelAgency.DTOs.Responses;
using TravelAgency.Interfaces.Services;
using TravelAgency.Security;

namespace TravelAgency.ApiControllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountApiController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                int userId = await _accountService.RegisterAsync(request);

                return Ok(new
                {
                    message = "Registration successful.",
                    userId
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

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _accountService.LoginAsync(request);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            // Store logged-in user information in Session.
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", user.RoleName);

            return Ok(new
            {
                message = "Login successful.",
                user = new UserResponse
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    RoleName = user.RoleName
                }
            });
        }

        // POST: api/account/logout
        [HttpPost("logout")]
        [SessionAuthorize]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return Ok(new
            {
                message = "Logout successful."
            });
        }

        // GET: api/account/profile
        [HttpGet("profile")]
        [SessionAuthorize]
        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized(new
                {
                    message = "Please log in first."
                });
            }

            return Ok(new
            {
                userId,
                fullName = HttpContext.Session.GetString("FullName"),
                email = HttpContext.Session.GetString("Email"),
                role = HttpContext.Session.GetString("Role")
            });
        }
    }
}