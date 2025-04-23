using Microsoft.AspNetCore.Mvc;

namespace YourAppName.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class RecipeController : Controller
	{
		public IActionResult RecipeIndex()
		{
			return View();
		}
	}
}
