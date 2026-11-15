using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TravelAgency.Security
{
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Method,
        AllowMultiple = false)]
    public class SessionAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;

        public SessionAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public Task OnAuthorizationAsync(
            AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session;

            int? userId = session.GetInt32("UserId");

            // Check whether user is logged in.
            if (userId == null)
            {
                context.Result = new UnauthorizedObjectResult(
                    new { message = "Please log in first." });

                return Task.CompletedTask;
            }

            // If no role is specified, any logged-in user is allowed.
            if (_roles.Length == 0)
                return Task.CompletedTask;

            string? userRole = session.GetString("Role");

            // Check user role.
            if (string.IsNullOrEmpty(userRole) ||
                !_roles.Contains(userRole, StringComparer.OrdinalIgnoreCase))
            {
                context.Result = new ObjectResult(
                    new { message = "You are not authorized to access this resource." })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }

            return Task.CompletedTask;
        }
    }
}