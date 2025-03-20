namespace Boilerplate.Beta.Core.Application.Messaging.Kafka
{
	public interface IKafkaProducer
	{
		Task PublishMessageAsync(string topic, string message);
	}
}
