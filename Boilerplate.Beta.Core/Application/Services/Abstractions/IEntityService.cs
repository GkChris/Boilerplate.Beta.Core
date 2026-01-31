using Boilerplate.Beta.Core.Application.Models.Entities;

namespace Boilerplate.Beta.Core.Application.Services.Abstractions
{
    /// <summary>
    /// Entity-specific service interface
    /// </summary>
    public interface IEntityService
    {
        Task<Entity?> GetByIdAsync(Guid id);
        Task<IEnumerable<Entity>> GetAllAsync();
        Task<Entity> AddAsync(Entity entity);
        Task<Entity> UpdateAsync(Entity entity);
        Task DeleteAsync(Guid id);
    }
}
