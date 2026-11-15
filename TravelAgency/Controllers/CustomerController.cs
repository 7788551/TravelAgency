using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
