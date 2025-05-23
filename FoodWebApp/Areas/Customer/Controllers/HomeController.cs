using System.Diagnostics;
using Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodWebApp.Areas.Customer.Controllers;

/// 
/// Customer Home Page Controller
/// 

[Area("Customer")]
public class HomeController : Controller
{
    private readonly IWebHostEnvironment _env;

    public HomeController(IWebHostEnvironment env)
    {
        _env = env;
    }

    public IActionResult Index()
    {
        // Path to the images folder
        var imageFolder = Path.Combine(_env.WebRootPath, "images\\carouselImages");
        // Get all image file paths (can add more extensions if needed)
        var imageFiles = Directory
            .GetFiles(imageFolder, "*.*")
            .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".jpeg"))
            .Select(file => "/images/carouselImages/" + Path.GetFileName(file))
            .ToList();

        ViewBag.ImagePaths = imageFiles;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
