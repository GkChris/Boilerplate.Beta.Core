using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Handlers
{
    public class KafkaMessageHandlers : IKafkaMessageHandler
    {
        private readonly ILogger<KafkaMessageHandlers> _logger;
        private readonly IEntityService _entityService;

        public KafkaMessageHandlers(IEntityService entityService, ILogger<KafkaMessageHandlers> logger)
        {
            _entityService = entityService;
            _logger = logger;
        }

        public async Task HandleGenericMessage(string topic, string message)
        {
            _logger.LogInformation("Processing message from topic '{Topic}': {Message}", topic, message);

            try
            {
                _logger.LogInformation("Successfully processed message from topic '{Topic}'", topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message for topic '{Topic}'", topic);
            }
        }
    }
}