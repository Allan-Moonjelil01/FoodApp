using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models
{
    public class MenuTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pick at least one meal")]
        public List<int> SelectedMealIds { get; set; } = new();

        public List<SelectListItem> AvailableMeals { get; set; } = new();
    }
}
