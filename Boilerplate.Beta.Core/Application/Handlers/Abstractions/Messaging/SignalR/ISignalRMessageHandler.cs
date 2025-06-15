namespace Boilerplate.Beta.Core.Application.Handlers.Abstractions.Messaging.SignalR
{
    public interface ISignalRMessageHandler
    {
        Task HandleMessageAsync(string message);

        bool CanHandleMessage(string message);
    }
}
