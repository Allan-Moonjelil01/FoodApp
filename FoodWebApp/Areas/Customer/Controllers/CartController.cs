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

        // POST: /Customer/Cart/RemoveFromCart/5
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _uow.Carts.GetByIdAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            await _uow.Carts.DeleteAsync(id);
            await _uow.CompleteAsync();
            return RedirectToAction(nameof(Index));  // Redirect back to the Cart page after removal
        }
    }
}
