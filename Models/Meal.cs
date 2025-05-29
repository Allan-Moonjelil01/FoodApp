namespace Models
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Available { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<MealType> MenuTypes { get; set; } = new List<MealType>();
    }
}