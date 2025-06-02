
namespace Models
{
    public class RecipeViewModel
    {
        public List<MealType> MealTypes { get; set; }
        public int SelectedTypeId { get; set; }
        public List<Meal> Meals { get; set; }
    }
}