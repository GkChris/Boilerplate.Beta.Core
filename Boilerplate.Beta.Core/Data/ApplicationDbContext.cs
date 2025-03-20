using Boilerplate.Beta.Core.Application.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Beta.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Entity> Entity { get; set; }
    }
}
