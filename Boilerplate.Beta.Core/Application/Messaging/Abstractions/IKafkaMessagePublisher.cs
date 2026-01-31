namespace Boilerplate.Beta.Core.Application.Handlers.Abstractions
{
    public interface IKafkaMessagePublisher
    {
        Task PublishKafkaMessage<T>(string topic, T message);
    }
}
