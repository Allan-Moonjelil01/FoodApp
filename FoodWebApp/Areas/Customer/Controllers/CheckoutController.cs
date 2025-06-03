using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using DataAccess;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DataAccess.Repository.IRepository;

[Area("Customer")]
public class CheckoutController : Controller
{
    private readonly ICartRepository _cartRepository;
    private readonly StripeSettings _stripeSettings;

    public CheckoutController(ICartRepository cartRepository, IOptions<StripeSettings> stripeSettings)
    {
        _cartRepository = cartRepository;
        _stripeSettings = stripeSettings.Value;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get UserId from Claims
        var cartItems = _cartRepository.GetList(c => c.UserId == userId).ToList();

        var totalAmount = cartItems.Sum(c => c.Price * c.Quantity);

        var vm = new CheckoutViewModel
        {
            PublishableKey = _stripeSettings.PublishableKey,
            TotalAmount = totalAmount
        };

        return View(vm);
    }
}
