using Boilerplate.Beta.Core.Application.Models.Entities;

namespace Boilerplate.Beta.Core.Application.Services.Abstractions
{
    /// <summary>
    /// Entity-specific service interface
    /// Extends base service with entity-specific operations
    /// </summary>
    public interface IEntityService : IService<Entity>
    {
        // Add entity-specific methods here if needed
        // e.g. Task<IEnumerable<Entity>> GetByStatusAsync(string status);
    }
}
