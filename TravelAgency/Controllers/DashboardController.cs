using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
