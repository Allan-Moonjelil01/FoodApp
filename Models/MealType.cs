using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MealType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        // navigation to MenuItems
        public ICollection<Meal> Meals { get; set; } = new List<Meal>();

    }
}
