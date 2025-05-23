using Microsoft.AspNetCore.Mvc;

namespace FoodWebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Stripe;
    using Stripe.Checkout;
    using DataAccess;
    using System.Threading.Tasks;
    using System.Linq;
    using FoodWebApp.Utility;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;

    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly StripeSettings _stripeSettings;

        public PaymentsController(IUnitOfWork uow, IOptions<StripeSettings> stripeSettings)
        {
            _uow = uow;
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            // Get current user's cart
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cartItems = await _uow.Carts.Query()
    .Where(c => c.UserId == userId)
    .Include(c => c.MenuItem)
    .ToListAsync();


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
                            Name = item.MenuItem.Name,
                        }
                    },
                    Quantity = item.Quantity
                }).ToList()
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Ok(new { sessionId = session.Id, url = session.Url });
        }
    }

}
