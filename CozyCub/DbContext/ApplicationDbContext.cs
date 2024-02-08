using CozyCub.Models.User;
using CozyCub.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace CozyCub
{
    public class ApplicationDbContext : DbContext
    {
        //configuration reference
        private readonly string _connectionString;

        public ApplicationDbContext(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString:DefaultConnection"];
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Roles)
                .HasDefaultValue("user");
        }


    }
}
