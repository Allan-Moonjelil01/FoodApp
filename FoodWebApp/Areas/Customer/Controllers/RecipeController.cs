using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FoodWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class RecipeController : Controller
    {
        private readonly IUnitOfWork _uow;
        public RecipeController(IUnitOfWork uow) => _uow = uow;

        // GET: /Customer/Recipe/RecipeIndex?typeId=3
        public async Task<IActionResult> RecipeIndex(int? typeId)
        {
            // 1) fetch all types
            var types = await _uow.MenuType
                .Query()
                .Select(mt => new MenuTypeDto { Id = mt.Id, Name = mt.Name })
                .ToListAsync();

            // default to first if none
            var selectedId = typeId ?? types.FirstOrDefault()?.Id ?? 0;

            // 2) load that type's meals
            var meals = await _uow.MenuType
                .Query()
                .Where(mt => mt.Id == selectedId)
                .Include(mt => mt.Meals)
                .SelectMany(mt => mt.Meals)
                .Select(mi => new MenuItemDto
                {
                    Id = mi.Id,
                    Name = mi.Name,
                    Description = mi.Description,
                    Price = mi.Price,
                    ImageUrl = mi.ImageUrl
                })
                .ToListAsync();

            // 3) build VM
            var vm = new RecipeViewModel
            {
                MenuTypes = types,
                SelectedTypeId = selectedId,
                Meals = meals
            };

            return View(vm);
        }
    }
}
