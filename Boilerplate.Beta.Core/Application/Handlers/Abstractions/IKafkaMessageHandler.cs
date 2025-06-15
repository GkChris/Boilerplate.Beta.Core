namespace Boilerplate.Beta.Core.Application.Handlers.Abstractions
{
    public interface IKafkaMessageHandler
    {
        Task HandleGenericMessage(string topic, string message);
    }
}
