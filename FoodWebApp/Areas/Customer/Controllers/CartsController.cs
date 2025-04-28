using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodWebApp.Controllers.Api
{
    [ApiController]
    [Route("api/cart-items")]  // Changed route to avoid conflict
    public class CartsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public CartsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // POST api/cart-items
        [HttpPost]
        [Authorize]  // Ensure the user is authenticated
        public async Task<IActionResult> Post([FromBody] CartItemDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Get the user ID from the claim
            var menuItem = await _uow.MenuItem.GetByIdAsync(dto.MenuItemId); // Get the menu item from the DB

            if (menuItem == null)
            {
                return NotFound(); // Return 404 if the menu item is not found
            }

            // Create a new cart item
            var cartItem = new Cart
            {
                UserId = userId,
                MenuItemId = dto.MenuItemId,
                Quantity = dto.Quantity,
                Price = menuItem.Price
            };

            await _uow.Carts.AddAsync(cartItem);  // Add the cart item to the database
            await _uow.CompleteAsync();  // Save changes to the database

            return Ok();  // Return success response
        }
    }
}