using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FoodWebApp.Models
{
    public class ProductVM
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public decimal Price { get; set; }

        // NEW: for the dropdown of menu types (or categories)
        public IEnumerable<SelectListItem> MenuTypeList { get; set; }

        // NEW: file upload binding
        [Display(Name = "Product Image")]
        public IFormFile? ImageFile { get; set; }

        // store the existing image path
        public string? ImageUrl { get; set; }

        [Required] public bool IsAvailable { get; set; }
    }
}
