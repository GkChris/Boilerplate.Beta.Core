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
            return await _entityRepository.GetAllAsync();
        }

        public async Task<Entity?> GetEntityByIdAsync(Guid id)
        {
            return await _entityRepository.GetByIdAsync(id);
        }

        public async Task<Entity> CreateEntityAsync(Entity entity)
        {
            return await _entityRepository.AddAsync(entity);
        }
    }
}
