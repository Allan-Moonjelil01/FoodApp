using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using DataAccess;
using Models;
using FoodWebApp.Utility;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Area("Customer")]
public class CheckoutController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly StripeSettings _stripeSettings;

    public CheckoutController(IUnitOfWork uow, IOptions<StripeSettings> stripeSettings)
    {
        _uow = uow;
        _stripeSettings = stripeSettings.Value;
    }

    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var cartItems = await _uow.Carts.Query()
            .Where(c => c.UserId == userId)
            .ToListAsync();

        var totalAmount = cartItems.Sum(c => c.Price * c.Quantity);

        var vm = new CheckoutViewModel
        {
            PublishableKey = _stripeSettings.PublishableKey,
            TotalAmount = totalAmount
        };

        return View(vm);
    }
}
