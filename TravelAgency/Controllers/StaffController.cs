using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class StaffController : Controller
    {
        // GET: /Staff/Index
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            string? role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (role != "Admin")
                return RedirectToAction("Index", "Dashboard");

            return View();
        }
    }
}