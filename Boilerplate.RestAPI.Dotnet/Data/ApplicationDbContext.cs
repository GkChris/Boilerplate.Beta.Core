using Microsoft.EntityFrameworkCore;

namespace Boilerplate.RestAPI.Dotnet.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor that accepts DbContextOptions to support dependency injection
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet properties for entities go here when you add them in the future, e.g.
        // public DbSet<User> Users { get; set; }
        // public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configurations go here when you need them.
            // For instance, to configure relationships, indexes, or constraints
        }
    }
}
