using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class PackageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
