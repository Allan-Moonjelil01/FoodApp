using Microsoft.AspNetCore.Mvc;

namespace FoodWebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using DataAccess;
    using System.Threading.Tasks;
    using System.Linq;
 
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;
    using DataAccess.Repository.IRepository;
    using Models;
    using DataAccess.Repository;
    using Stripe;
    using Stripe.Checkout;

    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly StripeSettings _stripeSettings;

        public PaymentsController(ICartRepository cartRepository, IOptions<StripeSettings> stripeSettings)
        {
            _cartRepository = cartRepository;
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            // Get current user's cart
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get UserId from Claims

            var cartItems = _cartRepository.GetAllAlongWithLinked()
                .Where(c => c.UserId == userId)
                .Include(c => c.Meal)
                .ToList();
    

            if (cartItems == null || !cartItems.Any())
                return BadRequest("Cart is empty.");

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                SuccessUrl = "https://localhost:7155/Customer/Order/OrderConfirmation?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://localhost:7155/Customer/Cart/Index",
                LineItems = cartItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",  // or your currency
                        UnitAmount = (long)(item.Price * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Meal.Name,
                        }
                    },
                    Quantity = item.Quantity
                }).ToList()
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            //var cartClearItems = _cartRepository.GetList(c => c.UserId == userId).ToList();
            _cartRepository.RemoveRange(cartItems);
            _cartRepository.Save();


            return Ok(new { sessionId = session.Id, url = session.Url });
        }

    
    }

}
