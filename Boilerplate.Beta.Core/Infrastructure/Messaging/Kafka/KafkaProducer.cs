using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly string _bootstrapServers;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(string bootstrapServers, ILogger<KafkaProducer> logger)
        {
            _bootstrapServers = bootstrapServers;
            _logger = logger;
        }

        public async Task ProduceMessageAsync(string topic, string message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                    _logger.LogError($"Message published: {result.Value}");

                }
                catch (ProduceException<Null, string> e)
                {
                    _logger.LogError($"Error publishing message: {e.Error.Reason}");
                }
            }
        }
    }
}