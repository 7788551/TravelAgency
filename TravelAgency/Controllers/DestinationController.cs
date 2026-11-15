using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class DestinationController : Controller
    {
        // GET: /Destination/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}