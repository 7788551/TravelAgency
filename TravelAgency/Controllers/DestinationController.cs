using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class DestinationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
