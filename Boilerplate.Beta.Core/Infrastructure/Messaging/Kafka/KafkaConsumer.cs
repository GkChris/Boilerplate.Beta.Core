using System.Collections.Concurrent;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka
{
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IConsumer<string, string> _consumer;
        private readonly ConcurrentDictionary<string, Func<string, Task>> _handlers = new();
        private readonly KafkaSettings _kafkaSettings;

        public KafkaConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaConsumer> logger)
        {
            _logger = logger;
            _kafkaSettings = kafkaSettings.Value;

            if (_kafkaSettings == null)
            {
                throw new ArgumentNullException(nameof(kafkaSettings), "Kafka _kafkaSettings cannot be null.");
            }

            if (string.IsNullOrEmpty(_kafkaSettings.BootstrapServers))
            {
                throw new ArgumentException("'BootstrapServers' is required.");
            }

            if (string.IsNullOrEmpty(_kafkaSettings.GroupId))
            {
                throw new ArgumentException("'GroupId' is required.");
            }

            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                GroupId = _kafkaSettings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe(_kafkaSettings.Topics ?? new List<string>());

            _logger.LogInformation($"Kafka Consumer initialized. Subscribed to topics: {string.Join(", ", _kafkaSettings.Topics ?? new List<string>())}");
        }

        public void Subscribe(string topic, Func<string, Task> messageHandler)
        {
            if (_handlers.TryAdd(topic, messageHandler))
            {
                _logger.LogInformation($"Subscribed dynamically to topic: {topic}");
            }
        }

        public void Unsubscribe(string topic)
        {
            if (_handlers.TryRemove(topic, out _))
            {
                _logger.LogInformation($"Unsubscribed from topic: {topic}");
            }
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kafka Consumer started.");

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(cancellationToken);
                        if (consumeResult?.Message != null && _handlers.TryGetValue(consumeResult.Topic, out var handler))
                        {
                            _logger.LogInformation($"Message received from topic {consumeResult.Topic}: {consumeResult.Message.Value}");
                            await handler(consumeResult.Message.Value);
                            _consumer.Commit(consumeResult);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("Kafka consumer stopping...");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Kafka Consumer Error: {ex.Message}");
                    }
                }
            }
            finally
            {
                _consumer.Close();
                _logger.LogInformation("Kafka Consumer closed.");
            }
        }
    }
}