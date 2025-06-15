using Boilerplate.Beta.Core.Application.Services.Abstractions.Messaging.Kafka;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using static Boilerplate.Beta.Core.Application.Shared.Constants.ColorConstants;

namespace Boilerplate.Beta.Core.Application.Services.Messaging.Kafka
{
    public class KafkaPublisherService : IKafkaPublisherService
    {
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<KafkaPublisherService> _logger;

        public KafkaPublisherService(IKafkaProducer kafkaProducer, ILogger<KafkaPublisherService> logger)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task PublishKafkaMessage<T>(string topic, T message)
        {
            try
            {
                var messageJson = JsonSerializer.Serialize(message);
                await _kafkaProducer.ProduceMessageAsync(topic, messageJson);
                _logger.LogInformation($"{AnsiColors.Blue}[Kafka Publisher] - {AnsiColors.Green}Success{AnsiColors.Reset}: Published message to topic '{topic}': {messageJson}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AnsiColors.Blue}[Kafka Publisher] - {AnsiColors.Red}Failure{AnsiColors.Reset}: Error publishing message to topic '{topic}': {ex.Message}");
            }
        }
    }
}