﻿using Boilerplate.Beta.Core.Application.Models.Entities;
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
            var entities = await _entityService.GetAllEntitiesAsync();

            if (entities == null)
            {
                return NotFound("No entities found.");
            }

            return Ok(entities);
        }

		[HttpGet("get/{id}")]
		public async Task<IActionResult> GetEntity(Guid id)
		{
            var entity = await _entityRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound("Entity not found.");
            }

            return Ok(entity);
        }

		[HttpPost("create")]
        public async Task<IActionResult> CreateEntity([FromBody] Entity payload)
        {
            if (payload == null)
            {
                return BadRequest("Entity data is required.");
            }

            var entity = await _entityRepository.AddAsync(payload);

            return Ok(entity);
        }
	}
}