namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions
{
    public interface IKafkaProducer
    {
        Task ProduceessageAsync(string topic, string message);
    }
}