using Boilerplate.Beta.Core.Application.Handlers.Messaging.Kafka;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        private readonly IKafkaConsumer _kafkaConsumer;
        private readonly ILogger<KafkaConsumerBackgroundService> _logger;
        private readonly KafkaSettings _kafkaSettings;
        private readonly IServiceScopeFactory _scopeFactory;

        public KafkaConsumerBackgroundService(
            IKafkaConsumer kafkaConsumer,
            ILogger<KafkaConsumerBackgroundService> logger,
            IOptions<KafkaSettings> kafkaSettings,
            IServiceScopeFactory scopeFactory)
        {
            _kafkaConsumer = kafkaConsumer;
            _logger = logger;
            _kafkaSettings = kafkaSettings.Value;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Kafka Consumer Background Service is running.");

            foreach (var topic in _kafkaSettings.Topics ?? new List<string>())
            {
                _kafkaConsumer.Subscribe(topic, message => HandleMessage(topic, message, stoppingToken));
            }

            await _kafkaConsumer.StartConsumingAsync(stoppingToken);
        }

        private async Task HandleMessage(string topic, string message, CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var messageHandlers = scope.ServiceProvider.GetRequiredService<KafkaMessageHandlers>();
                await messageHandlers.HandleGenericMessage(topic, message);
            }
        }
    }
}
