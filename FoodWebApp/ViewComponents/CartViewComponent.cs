using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;
using System.Security.Claims;
using System.Threading.Tasks;

public class CartViewComponent : ViewComponent
{
    private readonly ICartRepository _cartRepository;

    public CartViewComponent(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public IViewComponentResult Invoke()
    {
        var userId = (User as ClaimsPrincipal).FindFirstValue(ClaimTypes.NameIdentifier); // Get UserId from Claims
        int count = 0;

        if (!string.IsNullOrEmpty(userId))
        {
            var cartItems = _cartRepository.GetList(c => c.UserId == userId);
            count = cartItems.Sum(c => c.Quantity);
        }

        return View(count); // Pass count to view
    }
}
