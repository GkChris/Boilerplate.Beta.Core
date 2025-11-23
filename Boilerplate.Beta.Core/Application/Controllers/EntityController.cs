using Boilerplate.Beta.Core.Application.Models.Entities;
using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly IEntityService _entityService;

        public EntityController(IEntityService entityService)
        {
            _entityService = entityService;
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllEntities()
        {
            var entities = await _entityService.GetAllAsync();

            if (entities == null || !entities.Any())
            {
                return NotFound("No entities found.");
            }

            return Ok(entities);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetEntity(Guid id)
        {
            var entity = await _entityService.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound("Entity not found.");
            }

            return Ok(entity);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateEntity([FromBody] Entity payload)
        {
            var entity = await _entityService.AddAsync(payload);
            return Ok(entity);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateEntity(Guid id, [FromBody] Entity payload)
        {
            var existingEntity = await _entityService.GetByIdAsync(id);
            if (existingEntity == null)
            {
                return NotFound("Entity not found.");
            }

            var entity = await _entityService.UpdateAsync(payload);
            return Ok(entity);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEntity(Guid id)
        {
            var existingEntity = await _entityService.GetByIdAsync(id);
            if (existingEntity == null)
            {
                return NotFound("Entity not found.");
            }

            await _entityService.DeleteAsync(id);
            return NoContent();
        }
    }
}