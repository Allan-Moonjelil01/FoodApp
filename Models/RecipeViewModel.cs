using System.Collections.Generic;

namespace Models
{
    public class RecipeViewModel
    {
        // for sidebar
        public List<MenuTypeDto> MenuTypes { get; set; }
        public int SelectedTypeId { get; set; }

        // the cards
        public List<MenuItemDto> Meals { get; set; }
    }

    public class MenuTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
