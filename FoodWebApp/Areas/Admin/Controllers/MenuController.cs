using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
public class MenuController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
