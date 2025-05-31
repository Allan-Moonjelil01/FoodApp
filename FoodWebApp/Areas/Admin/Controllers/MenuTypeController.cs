using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;


[Area("Admin")]
public class MenuTypeController : Controller
{
    private readonly IMealTypeRepository _mealTypeRepository;
    private readonly IMealRepository _mealRepository;

    public MenuTypeController(IMealTypeRepository mealTypeRepository, IMealRepository mealRepository)
    {
        _mealTypeRepository = mealTypeRepository;
        _mealRepository = mealRepository;
    }

    public IActionResult Index()
    {
        //        List<MealType> menuTypes = new List<MealType>
        //{
        //    new MealType { Id = 1, Name = "Breakfast" ,Meals = new List<Meal>
        //        {
        //            new Meal { Id = 1, Name = "Pancakes", Description = "Fluffy pancakes with syrup" },
        //            new Meal { Id = 2, Name = "Omelette", Description = "Cheese and veggie omelette" }
        //        } },
        //    new MealType { Id = 2, Name = "Lunch" }};

        var menuTypes = _mealTypeRepository.GetAllMeals();
        return View(menuTypes);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new CreateMealTypeViewModel
        {
            AvailableMeals = _mealRepository.GetAll().Select(m => new SelectListItem(m.Name, m.Id.ToString())).ToList()
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMealTypeViewModel vm)
    {

        var mealType = new MealType
        {
            Name = vm.Name,
            Meals = _mealRepository.GetList(m => vm.SelectedMealIds.Contains(m.Id)).ToList()
        };
        _mealTypeRepository.Add(mealType);
        _mealTypeRepository.Save();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var mealType = _mealTypeRepository.Get(m => m.Id == id, includeProperties: "Meals");
        if (mealType == null) return NotFound();
        var allMeals = _mealRepository.GetAll();
        var vm = new CreateMealTypeViewModel
        {
            Id = mealType.Id,
            Name = mealType.Name,
            SelectedMealIds = mealType.Meals.Select(m => m.Id).ToList(),
            AvailableMeals = allMeals.Select(m => new SelectListItem(m.Name, m.Id.ToString())).ToList()
        };
        return View("create", vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CreateMealTypeViewModel vm)
    {
        var mealType = _mealTypeRepository.Get(m => m.Id == vm.Id, includeProperties: "Meals");
        if (mealType == null) return NotFound();

        mealType.Name = vm.Name;

        // Safely update many-to-many relation
        mealType.Meals.Clear();
        var selectedMeals = _mealRepository.GetList(m => vm.SelectedMealIds.Contains(m.Id)).ToList();
        foreach (var meal in selectedMeals)
        {
            mealType.Meals.Add(meal);
        }

        _mealTypeRepository.Save();
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var mealType = _mealTypeRepository.Get(m => m.Id == id, includeProperties: "Meals");
        if (mealType == null) return NotFound();

        // Clear the many-to-many relationships first
        mealType.Meals.Clear();

        _mealTypeRepository.Remove(mealType);
        _mealTypeRepository.Save();

        return RedirectToAction(nameof(Index));

    }
}

