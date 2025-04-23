using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	// Ensure the 'MenuItems' class is defined
	public class MenuItems
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string Available { get; set; }
	}

	/// 
	/// Used for Database Operations
	///
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<MenuItems> MenuItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MenuItems>().HasData(
				new MenuItems
				{
					Id = 1,
					Name = "Chapathi",
					Description = "Soft and fresh chapathi",
					Price = 12,
					Available = "Yes"
				},
				new MenuItems
				{
					Id = 2,
					Name = "Dosa",
					Description = "Crispy and delicious dosa",
					Price = 15,
					Available = "Yes"
				},
				new MenuItems
				{
					Id = 3,
					Name = "Idli",
					Description = "Steamed rice cakes",
					Price = 10,
					Available = "Yes"
				},
				new MenuItems
				{
					Id = 4,
					Name = "Puri",
					Description = "Deep-fried bread",
					Price = 20,
					Available = "Yes"
				}
			);
		}
	}

}
