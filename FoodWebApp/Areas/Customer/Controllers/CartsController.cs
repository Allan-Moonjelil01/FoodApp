using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodWebApp.Controllers.Api
{
    [ApiController]
    [Route("api/cart-items")]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMealRepository _mealRepository;

        public CartsController(
            ICartRepository cartRepository,
            IMealRepository mealRepository)
        {
            _cartRepository = cartRepository;
            _mealRepository = mealRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Cart cart)
        {

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var meal = _mealRepository.Get(m => m.Id == cart.MealId);
            if (meal == null)
                return NotFound("Meal not found.");

            // Set required values on the Cart object
            cart.UserId = userIdClaim;
            cart.Price = meal.Price;

             _cartRepository.Add(cart);
             _cartRepository.Save();

            return Ok(cart);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var cartItem = _cartRepository.Get(c=>c.Id == id);
            if (cartItem == null)
                return NotFound();

            _cartRepository.Remove(cartItem);
            _cartRepository.Save();

            return Ok();
        }
    }
}
