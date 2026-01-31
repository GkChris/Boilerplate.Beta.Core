using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Microsoft.Extensions.Logging;
using static Boilerplate.Beta.Core.Application.Shared.Constants.ColorConstants;

namespace Boilerplate.Beta.Core.Application.Handlers
{
    public class KafkaMessageConsumer : IKafkaMessageConsumer
    {
        private readonly ILogger<KafkaMessageConsumer> _logger;

        public KafkaMessageConsumer(ILogger<KafkaMessageConsumer> logger)
        {
            _logger = logger;
        }

        public async Task HandleGenericMessage(string topic, string message)
        {
            _logger.LogInformation("Processing message from topic '{Topic}': {Message}", topic, message);

            try
            {
                _logger.LogInformation($"{AnsiColors.BrightBlue}[Kafka Consumer] - {AnsiColors.Green}Success{AnsiColors.Reset}: Successfully processed message from topic '{topic}'");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AnsiColors.BrightBlue}[Kafka Consumer] - {AnsiColors.Red}Failure{AnsiColors.Reset}: Error processing message for topic '{topic}', {ex}");
            }
        }
    }
}