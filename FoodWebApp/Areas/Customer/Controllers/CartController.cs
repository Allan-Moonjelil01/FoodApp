using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace FoodWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        // GET: /Customer/Cart
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get UserId from Claims

            var cartItems = _cartRepository.GetAllAlongWithLinked()
                .Where(c => c.UserId == userId)
                .Include(c => c.Meal)
                .ToList();

            return View(cartItems);
        }
    }
}
