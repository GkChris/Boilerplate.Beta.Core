using Boilerplate.Beta.Core.Application.Models.Entities;
using Microsoft.AspNetCore.Mvc;

public class EntityController : Controller
{
	private readonly EntityService _entityService;
	private readonly IRepository<Entity> _entityRepository;

	public EntityController(EntityService entityService, IRepository<Entity> entityRepository)
	{
		_entityService = entityService;
		_entityRepository = entityRepository;
	}

	public async Task<IActionResult> GetAllEntities()
	{
		try
		{
			var entities = await _entityService.GetAllEntitiesAsync();

			if (entities == null)
			{
				return NotFound("No entities found.");
			}

			return Ok(entities); 
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	public async Task<IActionResult> GetEntity(Guid id)
	{
		try
		{
			var entity = await _entityRepository.GetByIdAsync(id);

			if (entity == null)
			{
				return NotFound("Entity not found.");
			}

			return Ok(entity);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}
}
