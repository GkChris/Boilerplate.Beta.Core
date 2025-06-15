namespace Boilerplate.Beta.Core.Application.Services.Abstractions.Messaging.Kafka
{
    public interface IKafkaPublisherService
    {
        Task PublishKafkaMessage<T>(string topic, T message);
    }
}
