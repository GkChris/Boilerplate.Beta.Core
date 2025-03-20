namespace Boilerplate.Beta.Core.Application.Messaging.Kafka
{
	public interface IKafkaConsumer
	{
		void Subscribe(string topic, Func<string, Task> messageHandler);
		void Unsubscribe(string topic);
	}
}