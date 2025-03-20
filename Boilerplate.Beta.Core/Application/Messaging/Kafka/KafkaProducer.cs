using Confluent.Kafka;

namespace Boilerplate.Beta.Core.Application.Messaging.Kafka
{
	public class KafkaProducer : IKafkaProducer
	{
		private readonly string _bootstrapServers;

		public KafkaProducer(string bootstrapServers)
		{
			_bootstrapServers = bootstrapServers;
		}

		public async Task PublishMessageAsync(string topic, string message)
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
					Console.WriteLine($"Message published: {result.Value}");
				}
				catch (ProduceException<Null, string> e)
				{
					Console.WriteLine($"Error publishing message: {e.Message}");
				}
			}
		}
	}
}
