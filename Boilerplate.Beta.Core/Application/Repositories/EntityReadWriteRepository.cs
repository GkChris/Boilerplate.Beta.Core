using Boilerplate.Beta.Core.Application.Models.Entities;
using Boilerplate.Beta.Core.Application.Repositories.Abstractions;
using Boilerplate.Beta.Core.Data;

namespace Boilerplate.Beta.Core.Application.Repositories
{
    public class EntityReadWriteRepository : ReadWriteRepository<Entity>, IEntityReadWriteRepository
    {
        public EntityReadWriteRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Add entity-specific queries here
    }
}
