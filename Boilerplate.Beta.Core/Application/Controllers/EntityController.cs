using Boilerplate.Beta.Core.Application.Models.Entities;
using Boilerplate.Beta.Core.Application.Repositories.Abstractions;
using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class EntityController : Controller
	{
		private readonly IEntityService _entityService;
		private readonly IRepository<Entity> _entityRepository;

		public EntityController(IEntityService entityService, IRepository<Entity> entityRepository)
		{
			_entityService = entityService;
			_entityRepository = entityRepository;
		}

		[HttpGet("get/all")]
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

		[HttpGet("get/{id}")]
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

		[HttpPost("create")]
        public async Task<IActionResult> CreateEntity([FromBody] Entity payload)
        {
            try
            {
                if (payload == null)
                {
                    return BadRequest("Entity data is required.");
                }

                var entity = await _entityRepository.AddAsync(payload);

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

		[HttpGet("test-success")]
		public async Task<IActionResult> TestSuccess()
		{
			try
			{
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet("test-error")]
		public async Task<IActionResult> TestError()
		{
			throw new NotImplementedException();
		}
	}
}