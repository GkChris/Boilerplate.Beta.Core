using Boilerplate.Beta.Core.Application.Models.Entities;
using Boilerplate.Beta.Core.Application.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Boilerplate.Beta.Core.Application.Handlers
{
	public class KafkaMessageHandlers
	{
		private readonly IEntityService _entityService;
		private readonly ILogger<KafkaMessageHandlers> _logger;

		public KafkaMessageHandlers(IEntityService entityService, ILogger<KafkaMessageHandlers> logger)
		{
			_entityService = entityService;
			_logger = logger;
		}

		public async Task HandleUserUpdate(string message)
		{
			try
			{
				var entity = JsonSerializer.Deserialize<Entity>(message);
				if (entity == null) throw new Exception("Invalid entity data received");

				_logger.LogInformation($"Processing Entity Update: {entity.Property1}");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error handling entity update: {ex.Message}");
			}
		}

		public async Task HandleOrderEvent(string message)
		{
			try
			{
				var entity = JsonSerializer.Deserialize<Entity>(message);
				if (entity == null) throw new Exception("Invalid entity data received");

				_logger.LogInformation($"Processing Entity Event: {entity.Property1}");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error handling entity event: {ex.Message}");
			}
		}
	}
}
