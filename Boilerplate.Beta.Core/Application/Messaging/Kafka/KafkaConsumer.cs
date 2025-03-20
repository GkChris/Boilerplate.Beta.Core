using Confluent.Kafka;

namespace Boilerplate.Beta.Core.Application.Messaging.Kafka
{
	public class KafkaConsumer : IKafkaConsumer
	{
		private readonly string _bootstrapServers;
		private readonly string _topic;
		private readonly string _groupId;

		public KafkaConsumer(string bootstrapServers, string topic, string groupId)
		{
			_bootstrapServers = bootstrapServers;
			_topic = topic;
			_groupId = groupId;
		}

		public void StartConsuming()
		{
			var config = new ConsumerConfig
			{
				GroupId = _groupId,
				BootstrapServers = _bootstrapServers,
				AutoOffsetReset = AutoOffsetReset.Earliest
			};

			using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
			{
				consumer.Subscribe(_topic);

				CancellationTokenSource cts = new CancellationTokenSource();
				Console.CancelKeyPress += (_, e) =>
				{
					e.Cancel = true;
					cts.Cancel();
				};

				try
				{
					while (!cts.Token.IsCancellationRequested)
					{
						try
						{
							var consumeResult = consumer.Consume(cts.Token);
							Console.WriteLine($"Consumed message: {consumeResult.Message.Value}");
						}
						catch (ConsumeException e)
						{
							Console.WriteLine($"Error occurred: {e.Error.Reason}");
						}
					}
				}
				catch (OperationCanceledException)
				{
					// Handle cancellation
				}
				finally
				{
					consumer.Close();
				}
			}
		}
	}
}
