using Boilerplate.Beta.Core.Application.Handlers;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        private readonly IKafkaConsumer _kafkaConsumer;
        private readonly KafkaMessageHandlers _messageHandlers;
        private readonly ILogger<KafkaConsumerBackgroundService> _logger;
        private readonly IConfiguration _configuration;

        public KafkaConsumerBackgroundService(
            IKafkaConsumer kafkaConsumer,
            KafkaMessageHandlers messageHandlers,
            ILogger<KafkaConsumerBackgroundService> logger,
            IConfiguration configuration)
        {
            _kafkaConsumer = kafkaConsumer;
            _messageHandlers = messageHandlers;
            _logger = logger;
            _configuration = configuration;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Kafka Consumer Background Service is running.");

            var topics = _configuration.GetSection("Kafka:Topics").Get<string[]>();
            foreach (var topic in topics)
            {
                _kafkaConsumer.Subscribe(topic, (message) => _messageHandlers.HandleGenericMessage(topic, message));
            }

            return Task.CompletedTask;
        }
    }
}