using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models;

namespace Models
{
	public class MenuItem
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Description is required")]
		public string Description { get; set; }
		[Required(ErrorMessage = "Price is required")]
		public decimal Price { get; set; }
		[Required(ErrorMessage = "Image URL is required")]
		public string ImageUrl { get; set; }
		[Required(ErrorMessage = "IsAvailable is required")]
		public bool IsAvailable { get; set; }
	}
}
