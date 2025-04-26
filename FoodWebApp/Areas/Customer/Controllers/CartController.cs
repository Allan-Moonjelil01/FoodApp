using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]  // Ensure that only customers can access the cart
    [Route("api/customer/[controller]")]  // API route for customer area
    [ApiController]  // Mark this as an API controller
    public class CartController : ControllerBase  // Use ControllerBase for API
    {
        private readonly IUnitOfWork _uow;

        public CartController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // Add an item to the cart
        [HttpPost("add")]  // Post method to add an item to the cart
        public async Task<IActionResult> AddToCart(int menuItemId, int quantity)
        {
            var menuItem = await _uow.MenuItem.GetByIdAsync(menuItemId);  // Get the menu item by ID
            if (menuItem == null)
            {
                return NotFound();  // Handle case where item is not found
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;  // Get the current user's ID
            if (userId == null)
            {
                return Unauthorized();  // Handle case where user is not logged in
            }

            var cartItem = new DataAccess.Cart
            {
                UserId = int.Parse(userId),
                MenuItemId = menuItemId,
                Quantity = quantity,
                Price = menuItem.Price
            };

            await _uow.Carts.AddAsync(cartItem);
            await _uow.CompleteAsync();  // Save changes to the database

            return RedirectToAction("Index");  // Redirect back to the menu page or cart page
        }



        // View items in the cart
        [HttpGet("index")]  // Get method to retrieve cart items
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;  // Get the current user's ID
            if (userId == null)
            {
                return Unauthorized(new { Message = "User is not authenticated" });  // Handle case where user is not logged in
            }

            var cartItems = await _uow.Carts.Query()
                                             .Where(c => c.UserId == int.Parse(userId))
                                             .Include(c => c.MenuItem)  // Include related menu items
                                             .ToListAsync();
            return Ok(cartItems);  // Return cart items in response
        }

        // Remove an item from the cart
        [HttpPost("remove")]  // Post method to remove an item from the cart
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            var cartItem = await _uow.Carts.GetByIdAsync(cartId);  // Use GetByIdAsync instead of Get
            if (cartItem == null)
            {
                return NotFound(new { Message = "Cart item not found" });  // Handle case where cart item is not found
            }

            await _uow.Carts.DeleteAsync(cartId);  // Use DeleteAsync here
            await _uow.CompleteAsync();  // Save changes

            return Ok(new { Message = "Item removed from cart successfully" });  // Return success message
        }
    }
}
