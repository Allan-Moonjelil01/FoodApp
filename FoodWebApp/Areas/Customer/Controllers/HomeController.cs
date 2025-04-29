using Microsoft.AspNetCore.Mvc;

namespace FoodWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        // GET: /Customer or /Customer/Home/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}
