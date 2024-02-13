using CozyCub.Models.CartModels;
using CozyCub.Models.Classification;
using CozyCub.Models.Orders;
using CozyCub.Models.ProductModels;
using CozyCub.Models.UserModels;
using CozyCub.Models.Wishlist;
using Microsoft.EntityFrameworkCore;

namespace CozyCub
{
    /// <summary>
    /// Represents the application's database context, responsible for interacting with the underlying database.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DefaultConnection"];
        }

        /// <summary>
        /// Represents the collection of users in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Represents the collection of products in the database.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Represents the collection of items in the user's shopping cart.
        /// </summary>
        public DbSet<Cart> Cart { get; set; }

        /// <summary>
        /// Represents the collection of categories in the database.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Represents the collection of items in a user's shopping cart.
        /// </summary>
        public DbSet<CartItem> CartItems { get; set; }

        /// <summary>
        /// Represents the collection of items in a user's wishlist.
        /// </summary>
        public DbSet<WishList> WishLists { get; set; }

        /// <summary>
        /// Represents the collection of orders in the database.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Represents the collection of items in an order.
        /// </summary>
        public DbSet<OrderedItem> OrderedItems { get; set; }

        /// <summary>
        /// Configures the database connection and options.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used to configure the database context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        /// <summary>
        /// Configures relationships and constraints between database entities.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure entity relationships.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User and Cart Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId);

            // Cart and CartItem Relationship
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(c => c.cart)
                .HasForeignKey(c => c.CartId);

            // CartItem and Product Relationship
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId);

            // Default value for User.Role
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("User");

            // Category and Product Relationship
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(c => c.CategoryId);

            // WishList and User Relationship
            modelBuilder.Entity<WishList>()
                .HasOne(w => w.User)
                .WithMany(u => u.WishLists)
                .HasForeignKey(w => w.UserId);

            // WishList and Product Relationship
            modelBuilder.Entity<WishList>()
                .HasOne(w => w.Product)
                .WithMany()
                .HasForeignKey(w => w.ProductId);

            // Order and User Relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.user)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            // OrderedItem and Order Relationship
            modelBuilder.Entity<OrderedItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            // OrderedItem and Product Relationship
            modelBuilder.Entity<OrderedItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // Default value for User.Banned
            modelBuilder.Entity<User>()
                .Property(u => u.Banned)
                .HasDefaultValue(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
