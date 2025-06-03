using Microsoft.AspNetCore.Mvc;

namespace FoodWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        // GET: /Customer/Order/OrderConfirmation
        public IActionResult OrderConfirmation(string session_id)
        {
            ViewData["SessionId"] = session_id;
            return View();
        }
    }

}
