using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public  class CreateMealTypeViewModel
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
