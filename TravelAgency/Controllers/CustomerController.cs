using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class CustomerController : Controller
    {
        // GET: /Customer/Index
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            string? role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (role != "Customer")
                return RedirectToAction("Index", "Dashboard");

            return View();
        }
    }
}