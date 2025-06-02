using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace FoodWebApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMealTypeRepository _mealTypeRepo;
        private readonly IMealRepository _mealRepo;

        public MenuController(IMealTypeRepository mealTypeRepo, IMealRepository mealRepo)
        {
            _mealTypeRepo = mealTypeRepo;
            _mealRepo = mealRepo;
        }

        // GET: /Menu
        public async Task<IActionResult> Index()
        {
            // Fetch all MealTypes with their included Meals
            var mealTypesWithMeals = _mealTypeRepo.GetAllMeals();

            // Return them to the view
            return View(mealTypesWithMeals);
        }

        // Optional: Add other actions as needed
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get a specific meal with its meal type
            var meal = _mealTypeRepo.Get(m => m.Id == id, "Meals");

            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }
    }
}