namespace Models.Api
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }
}

// Models/Api/MenuItemCreateDto.cs
namespace Models.Api
{
    public class MenuItemCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }
}

// Models/Api/MenuItemUpdateDto.cs
namespace Models.Api
{
    public class MenuItemUpdateDto : MenuItemCreateDto
    {
        public int Id { get; set; }
    }
}
