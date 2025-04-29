// Models/Api/IdNameDto.cs
namespace Models.Api
{
    public class IdNameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

// Models/Api/MenuTypeDto.cs
namespace Models.Api
{
    public class MenuTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<IdNameDto> Meals { get; set; }
    }
}

// Models/Api/MenuTypeCreateDto.cs
namespace Models.Api
{
    public class MenuTypeCreateDto
    {
        public string Name { get; set; }
        public List<int> SelectedMealIds { get; set; }
    }
}

// Models/Api/MenuTypeUpdateDto.cs
namespace Models.Api
{
    public class MenuTypeUpdateDto : MenuTypeCreateDto
    {
        public int Id { get; set; }
    }
}
