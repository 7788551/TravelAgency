using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
