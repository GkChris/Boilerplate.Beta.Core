namespace Boilerplate.Beta.Core.Application.Handlers.Abstractions
{
    public interface IWebSocketMessageHandler
    {
        Task HandleMessageAsync(string clientId, string message);

        bool CanHandleMessage(string message); 
    }
}
