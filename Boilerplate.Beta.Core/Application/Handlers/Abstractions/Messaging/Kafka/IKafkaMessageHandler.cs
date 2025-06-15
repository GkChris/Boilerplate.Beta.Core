namespace Boilerplate.Beta.Core.Application.Handlers.Abstractions.Messaging.Kafka
{
    public interface IKafkaMessageHandler
    {
        Task HandleGenericMessage(string topic, string message);
    }
}
