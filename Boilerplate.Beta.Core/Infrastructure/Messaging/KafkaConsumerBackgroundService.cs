using Boilerplate.Beta.Core.Application.Messaging.Kafka;
using Microsoft.Extensions.Hosting;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        private readonly IKafkaConsumer _kafkaConsumer;

        public KafkaConsumerBackgroundService(IKafkaConsumer kafkaConsumer)
        {
            _kafkaConsumer = kafkaConsumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => _kafkaConsumer.StartConsuming());
            return Task.CompletedTask;
        }
    }
}
