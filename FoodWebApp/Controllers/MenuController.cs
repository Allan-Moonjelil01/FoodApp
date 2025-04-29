// File: Controllers/MenuController.cs
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace FoodWebApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly IUnitOfWork _uow;

        public MenuController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: /Menu
        public async Task<IActionResult> Index()
        {
            // 1) Fetch all MenuItems from the database
            var menuItems = await _uow.MenuItem.GetAllAsync();

            // 2) Map DataAccess.MenuItem → Models.MenuItem
            var model = menuItems.Select(m => new DataAccess.MenuItem
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                Available = m.Available
            }).ToList();

            // 3) Render Views/Menu/Index.cshtml
            return View(model);
        }
    }
}
