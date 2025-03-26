using Boilerplate.Beta.Core.Application.Handlers;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        private readonly IKafkaConsumer _kafkaConsumer;
        private readonly KafkaMessageHandlers _messageHandlers;
        private readonly ILogger<KafkaConsumerBackgroundService> _logger;
        private readonly KafkaSettings _kafkaSettings;

        public KafkaConsumerBackgroundService(
            IKafkaConsumer kafkaConsumer,
            KafkaMessageHandlers messageHandlers,
            ILogger<KafkaConsumerBackgroundService> logger,
            IOptions<KafkaSettings> kafkaSettings)
        {
            _kafkaConsumer = kafkaConsumer;
            _messageHandlers = messageHandlers;
            _logger = logger;
            _kafkaSettings = kafkaSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Kafka Consumer Background Service is running.");

            foreach (var topic in _kafkaSettings.Topics ?? new List<string>())
            {
                _kafkaConsumer.Subscribe(topic, message => _messageHandlers.HandleGenericMessage(topic, message));
            }

            await _kafkaConsumer.StartConsumingAsync(stoppingToken);
        }
    }
}
