using System.Collections.Concurrent;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka
{
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IConsumer<string, string> _consumer;
        private readonly ConcurrentDictionary<string, Func<string, Task>> _handlers = new();
        private readonly CancellationTokenSource _cts = new();

        public KafkaConsumer(IConfiguration configuration, ILogger<KafkaConsumer> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var topics = _configuration.GetSection("Kafka:Topics").Get<string[]>() ?? Array.Empty<string>();

            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = _configuration["Kafka:ConsumerGroup"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe(topics);

            _logger.LogInformation($"Kafka Consumer initialized. Subscribed to topics: {string.Join(", ", topics)}");

            Task.Run(() => StartConsuming(), _cts.Token);
        }

        public void Subscribe(string topic, Func<string, Task> messageHandler)
        {
            if (_handlers.TryAdd(topic, messageHandler))
            {
                _consumer.Subscribe(new List<string>(_handlers.Keys));
                _logger.LogInformation($"Subscribed dynamically to topic: {topic}");
            }
        }

        public void Unsubscribe(string topic)
        {
            if (_handlers.TryRemove(topic, out _))
            {
                _consumer.Subscribe(new List<string>(_handlers.Keys));
                _logger.LogInformation($"Unsubscribed from topic: {topic}");
            }
        }

        private async Task StartConsuming()
        {
            _logger.LogInformation("Kafka Consumer started.");

            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(_cts.Token);
                    if (consumeResult != null && _handlers.TryGetValue(consumeResult.Topic, out var handler))
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

        public void Stop()
        {
            _logger.LogInformation("Kafka Consumer is shutting down.");
            _cts.Cancel();
            _consumer.Close();
        }
    }
}