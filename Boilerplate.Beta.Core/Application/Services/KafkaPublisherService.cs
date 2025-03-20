using System.Text.Json;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Services
{
    public class KafkaPublisherService
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
                _logger.LogInformation($"Published message to topic '{topic}': {messageJson}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error publishing message to topic '{topic}': {ex.Message}");
            }
        }
    }
}