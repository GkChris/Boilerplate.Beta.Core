namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions
{
    public interface IKafkaProducer
    {
        Task PublishMessageAsync(string topic, string message);
    }
}