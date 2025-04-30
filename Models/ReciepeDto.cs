// Models/Api/RecipeTypeDto.cs
namespace Models.Api
{
    public class RecipeTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

// Models/Api/RecipeMealDto.cs
namespace Models.Api
{
    public class RecipeMealDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
