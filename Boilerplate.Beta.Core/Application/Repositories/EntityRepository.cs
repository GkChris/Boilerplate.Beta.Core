using Boilerplate.Beta.Core.Application.Models.Entities;
using Boilerplate.Beta.Core.Application.Repositories.Abstractions;
using Boilerplate.Beta.Core.Data;

namespace Boilerplate.Beta.Core.Application.Repositories
{
    public class EntityRepository : Repository<Entity>, IEntityRepository
    {
        public EntityRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Example of an entity-specific query:
        //public async Task<IEnumerable<Entity>> GetByStatusAsync(string status)
        //{
        //    return await _dbSet.Where(e => e.Property1 == status).ToListAsync();
        //}
    }
}