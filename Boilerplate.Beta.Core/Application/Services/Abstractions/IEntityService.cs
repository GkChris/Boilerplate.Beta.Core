using Boilerplate.Beta.Core.Application.Models.Entities;

namespace Boilerplate.Beta.Core.Application.Services.Abstractions
{
    public interface IEntityService
    {
        Task<IEnumerable<Entity>> GetAllEntitiesAsync();
    }
}
