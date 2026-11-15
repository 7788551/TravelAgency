using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
