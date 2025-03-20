using Boilerplate.Beta.Core.Application.Models.Entities;

public class EntityService
{
	private readonly IRepository<Entity> _entityRepository;

	public EntityService(IRepository<Entity> entityRepository)
	{
		_entityRepository = entityRepository;
	}

	public async Task<IEnumerable<Entity>> GetAllEntitiesAsync()
	{
		var entities = await _entityRepository.GetAllAsync();

		var activeEntities = entities.Where(e => e.Property2 == "Example").ToList();

		return activeEntities;
	}
}
