using Boilerplate.Beta.Core.Application.Models.Entities;
using Boilerplate.Beta.Core.Application.Repositories.Abstractions;
using Boilerplate.Beta.Core.Application.Services.Abstractions;

namespace Boilerplate.Beta.Core.Application.Services
{
    public class EntityService : IEntityService
	{
		private readonly IRepository<Entity> _entityRepository;

		public EntityService(IRepository<Entity> entityRepository)
		{
			_entityRepository = entityRepository;
		}

		public async Task<IEnumerable<Entity>> GetAllEntitiesAsync()
		{
			var entities = await _entityRepository.GetAllAsync();

			return entities;
		}
	}
}
