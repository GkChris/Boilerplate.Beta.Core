namespace Boilerplate.Beta.Core.Application.Handlers.Abstractions
{
    public interface ISignalRMessagePublisher
    {
        Task SendMessageToAllAsync(string message);
        Task SendMessageToClientAsync(string targetClientId, string message);
    }
}
