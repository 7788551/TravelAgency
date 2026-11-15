using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class PackageController : Controller
    {
        // GET: /Package/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}