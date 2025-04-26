using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace FoodWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;

        public HomeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // Display the list of menu items  
        public async Task<IActionResult> Index()
        {
            // Fetch all MenuItems from the database using DataAccess.MenuItems  
            var menuItems = await _uow.MenuItem.GetAllAsync();

            // Map DataAccess.MenuItems to Models.MenuItem  
            var model = menuItems.Select(m => new Models.MenuItem
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsAvailable = m.Available == "Yes"
            }).ToList();

            return View(model);  // Pass the mapped list to the view  
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
