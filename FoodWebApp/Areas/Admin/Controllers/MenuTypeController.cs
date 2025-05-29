using Microsoft.AspNetCore.Mvc;
using Models;


[Area("Admin")]
public class MenuTypeController : Controller
    {
    public IActionResult Index()
    {
        List<MealType> menuTypes = new List<MealType>
{
    new MealType { Id = 1, Name = "Breakfast" ,Meals = new List<Meal>
        {
            new Meal { Id = 1, Name = "Pancakes", Description = "Fluffy pancakes with syrup" },
            new Meal { Id = 2, Name = "Omelette", Description = "Cheese and veggie omelette" }
        } },
    new MealType { Id = 2, Name = "Lunch" }};
        return View(menuTypes);
    }
}

