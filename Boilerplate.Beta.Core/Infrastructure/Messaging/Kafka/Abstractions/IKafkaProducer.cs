namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions
{
    public interface IKafkaProducer
    {
        Task ProduceMessageAsync(string topic, string message);
    }
}