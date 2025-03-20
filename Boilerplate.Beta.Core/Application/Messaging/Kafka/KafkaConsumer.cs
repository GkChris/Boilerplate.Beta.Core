using System.Collections.Concurrent;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Messaging.Kafka
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

			var config = new ConsumerConfig
			{
				BootstrapServers = _configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
				GroupId = _configuration["Kafka:GroupId"] ?? "dynamic-group",
				AutoOffsetReset = AutoOffsetReset.Earliest,
				EnableAutoCommit = false
			};

			_consumer = new ConsumerBuilder<string, string>(config).Build();
			Task.Run(() => StartConsuming(), _cts.Token);
		}

		public void Subscribe(string topic, Func<string, Task> messageHandler)
		{
			if (_handlers.TryAdd(topic, messageHandler))
			{
				_consumer.Subscribe(new List<string>(_handlers.Keys));
				_logger.LogInformation($"Subscribed to topic: {topic}");
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
			_cts.Cancel();
			_consumer.Close();
		}
	}
}