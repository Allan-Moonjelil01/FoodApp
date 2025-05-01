using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace FoodWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _uow;

        public CartController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: /Customer/Cart
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));  // Get UserId from Claims
            var cartItems = await _uow.Carts.Query()
                .Where(c => c.UserId == userId)
                .Include(c => c.MenuItem)
                .ToListAsync();

            return View(cartItems);  // Passing the cart items to the view
        }
    }
}
