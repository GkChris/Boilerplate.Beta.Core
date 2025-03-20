using Boilerplate.Beta.Core.Application.Models.Entities;

namespace Boilerplate.Beta.Core.Application.Services
{
	public interface IEntityService
	{
		Task<IEnumerable<Entity>> GetAllEntitiesAsync();
	}
}
