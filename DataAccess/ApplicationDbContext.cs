using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        IQueryable<T> Query();
    }
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        IGenericRepository<MenuItem> MenuItem { get; }
        IGenericRepository<MenuType> MenuType { get; }
        IGenericRepository<Cart> Carts { get; }
        Task<int> CompleteAsync();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public IGenericRepository<MenuItem> MenuItem => new GenericRepository<MenuItem>(_context);
        public IGenericRepository<Cart> Carts => new GenericRepository<Cart>(_context);
        public IGenericRepository<MenuType> MenuType => new GenericRepository<MenuType>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();  // This allows querying the DbSet directly
        }
    }

    // Ensure the 'MenuItems' class is defined
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Available { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<MenuType> MenuTypes { get; set; } = new List<MenuType>();
    }

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


        public DataAccess.User User { get; set; }
        public MenuItem MenuItem { get; set; }
    }

    public class MenuType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // navigation to MenuItems
        public ICollection<MenuItem> Meals { get; set; } = new List<MenuItem>();
    }

    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int MenuItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public decimal TotalAmount { get; set; }

        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public class CartItemDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
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
        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<DataAccess.User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }

        // Defining relationships and initial data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set precision for Price in MenuItems
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasPrecision(18, 2); // Precision: 18 digits, Scale: 2 decimal places

            // Set precision for Price in Cart
            modelBuilder.Entity<Cart>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            // Existing configurations
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    Id = 1,
                    Name = "Chapathi",
                    Description = "Soft and fresh chapathi",
                    Price = 12,
                    Available = "Yes",
                    ImageUrl = "https://forkify-api.herokuapp.com/images/steakhousepizza0b87.jpg"
                },
                new MenuItem
                {
                    Id = 2,
                    Name = "Dosa",
                    Description = "Crispy and delicious dosa",
                    Price = 15,
                    Available = "Yes",
                    ImageUrl = "https://forkify-api.herokuapp.com/images/steakhousepizza0b87.jpg"
                },
                new MenuItem
                {
                    Id = 3,
                    Name = "Idli",
                    Description = "Steamed rice cakes",
                    Price = 10,
                    Available = "Yes",
                    ImageUrl = "https://forkify-api.herokuapp.com/images/steakhousepizza0b87.jpg"
                },
                new MenuItem
                {
                    Id = 4,
                    Name = "Puri",
                    Description = "Deep-fried bread",
                    Price = 20,
                    Available = "Yes",
                    ImageUrl = "https://forkify-api.herokuapp.com/images/steakhousepizza0b87.jpg"
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

            modelBuilder.Entity<MenuType>()
                .HasMany(mt => mt.Meals)
                .WithMany(mi => mi.MenuTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "MenuTypeItem",
                    j => j.HasOne<MenuItem>().WithMany().HasForeignKey("MenuItemId"),
                    j => j.HasOne<MenuType>().WithMany().HasForeignKey("MenuTypeId"),
                    j => j.ToTable("MenuTypeItems")
                );

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany()
                .HasForeignKey(oi => oi.MenuItemId);
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
