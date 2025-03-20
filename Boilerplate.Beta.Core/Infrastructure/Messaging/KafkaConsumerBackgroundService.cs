using Boilerplate.Beta.Core.Application.Handlers;
using Boilerplate.Beta.Core.Application.Messaging.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging
{
	public class KafkaConsumerBackgroundService : BackgroundService
	{
		private readonly IKafkaConsumer _kafkaConsumer;
		private readonly KafkaMessageHandlers _messageHandlers;
		private readonly ILogger<KafkaConsumerBackgroundService> _logger;

		public KafkaConsumerBackgroundService(
			IKafkaConsumer kafkaConsumer,
			KafkaMessageHandlers messageHandlers,
			ILogger<KafkaConsumerBackgroundService> logger)
		{
			_kafkaConsumer = kafkaConsumer;
			_messageHandlers = messageHandlers;
			_logger = logger;
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Kafka Consumer Background Service is running.");

			_kafkaConsumer.Subscribe("user-updates", _messageHandlers.HandleUserUpdate);
			_kafkaConsumer.Subscribe("order-events", _messageHandlers.HandleOrderEvent);

			return Task.CompletedTask;
		}
	}
}