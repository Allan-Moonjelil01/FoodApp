using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace FoodWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class RecipeController : Controller
    {
        private readonly IMealTypeRepository _mealTypeRepo;

        public RecipeController(IMealTypeRepository mealTypeRepo)
        {
            _mealTypeRepo = mealTypeRepo;
        }

        // GET: /Customer/Recipe/RecipeIndex?typeId=3
        public async Task<IActionResult> RecipeIndex(int? typeId)
        {
            // 1) Get all meal types
            var mealTypes = await Task.Run(() => _mealTypeRepo.GetAll().ToList());

            // 2) Determine selected type ID
            var selectedId = typeId ?? mealTypes.FirstOrDefault()?.Id ?? 0;

            // 3) Get selected type with its meals
            var selectedType = _mealTypeRepo.Get(
                mt => mt.Id == selectedId,
                includeProperties: "Meals"
            );

            // 4) Prepare ViewModel
            var vm = new RecipeViewModel
            {
                MealTypes = mealTypes,
                SelectedTypeId = selectedId,
                Meals = selectedType?.Meals.ToList() ?? new List<Meal>()
            };

            return View(vm);
        }
    }
}
