using Microsoft.AspNetCore.Mvc;

namespace FoodWebApp.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
