using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class Cart
    {
        public int Id { get; set; }

       
        public string? UserId { get; set; }

        [Required]
        public int MealId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        // Prevent binding and validation


        [ForeignKey(nameof(UserId))]
        public ApplicationUser? ApplicationUser { get; set; }


        [ForeignKey(nameof(MealId))]
        public Meal? Meal { get; set; }
    }
}
