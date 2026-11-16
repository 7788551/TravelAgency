using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class CustomerController : Controller
    {
        // GET: /Customer/Index
        [HttpGet]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            string? role = HttpContext.Session.GetString("Role");

            // Check login
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Only customers can access this page
            if (role != "Customer")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }
    }
}
