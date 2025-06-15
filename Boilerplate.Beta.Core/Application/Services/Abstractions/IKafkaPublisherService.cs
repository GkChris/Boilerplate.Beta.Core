namespace Boilerplate.Beta.Core.Application.Services.Abstractions
{
    public interface IKafkaPublisherService
    {
        Task PublishKafkaMessage<T>(string topic, T message);
    }
}
