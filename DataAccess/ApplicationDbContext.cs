using Microsoft.EntityFrameworkCore;
using System;

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

    // Ensure the 'User' class is defined (simplified example, you might already have a more complete User entity)
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; } // 0 = Admin, 1 = Customer
    }

    // Cart class representing the user's cart entries
    public class Cart
    {
        public int Id { get; set; }           
        public int UserId { get; set; }        
        public int MenuItemId { get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Navigation properties
        //public User User { get; set; }
        //public MenuItems MenuItem { get; set; }

        // Fully qualified User class
        public DataAccess.User User { get; set; }
        public MenuItems MenuItem { get; set; }
    }

    /// <summary>
    /// Used for Database Operations
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for the tables
        public DbSet<MenuItems> MenuItems { get; set; }
        //public DbSet<User> Users { get; set; }

        public DbSet<DataAccess.User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }

        // Defining relationships and initial data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set precision for Price in MenuItems
            modelBuilder.Entity<MenuItems>()
                .Property(m => m.Price)
                .HasPrecision(18, 2); // Precision: 18 digits, Scale: 2 decimal places

            // Set precision for Price in Cart
            modelBuilder.Entity<Cart>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            // Existing configurations
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

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.MenuItem)
                .WithMany()
                .HasForeignKey(c => c.MenuItemId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("YourConnectionString", b => b.MigrationsAssembly("FoodWebApp"))
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            }
        }

    }
}
