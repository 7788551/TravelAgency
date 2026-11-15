using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class DashboardController : Controller
    {
        // GET: /Dashboard/Index
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            ViewBag.FullName =
                HttpContext.Session.GetString("FullName");

            ViewBag.Role =
                HttpContext.Session.GetString("Role");

            return View();
        }
    }
}