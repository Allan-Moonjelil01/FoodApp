using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodWebApp.Controllers.Api
{
    [ApiController]
    [Route("api/cart-items")]
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
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
            var menuItem = await _uow.MenuItem.GetByIdAsync(dto.MenuItemId);

            if (menuItem == null)
            {
                return NotFound();
            }

            // Create a new cart item
            var cartItem = new Cart
            {
                UserId = userId,
                MenuItemId = dto.MenuItemId,
                Quantity = dto.Quantity,
                Price = menuItem.Price
            };

            await _uow.Carts.AddAsync(cartItem);  
            await _uow.CompleteAsync();  

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var cartItem = await _uow.Carts.GetByIdAsync(id);
            if (cartItem == null) return NotFound();

            await _uow.Carts.DeleteAsync(id);
            await _uow.CompleteAsync();
            return Ok();
        }

    }
}