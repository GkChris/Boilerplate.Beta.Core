using Boilerplate.Beta.Core.Application.Models.Entities;

namespace Boilerplate.Beta.Core.Application.Repositories.Abstractions
{
    public interface IEntityReadWriteRepository : IReadWriteRepository<Entity>
    {
        // Add entity-specific queries here
    }
}
