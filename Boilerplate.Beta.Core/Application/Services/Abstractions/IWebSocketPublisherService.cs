namespace Boilerplate.Beta.Core.Application.Services.Abstractions
{
    public interface IWebSocketPublisherService
    {
        Task SendMessageToAllAsync(string message);
        Task SendMessageToClientAsync(string clientId, string message);
    }
}
