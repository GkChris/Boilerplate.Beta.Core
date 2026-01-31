namespace Boilerplate.Beta.Core.Application.Handlers.Abstractions
{
    public interface IKafkaMessageConsumer
    {
        Task HandleGenericMessage(string topic, string message);
    }
}
