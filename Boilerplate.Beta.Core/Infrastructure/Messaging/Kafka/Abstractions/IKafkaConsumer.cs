namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions
{
    public interface IKafkaConsumer
    {
        void Subscribe(string topic, Func<string, Task> messageHandler);
        void Unsubscribe(string topic);
    }
}