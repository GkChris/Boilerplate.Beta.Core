namespace Boilerplate.Beta.Core.Application.Handlers.Abstractions
{
    public interface ISignalRMessageHandler
    {
        Task HandleMessageAsync(string message);

        bool CanHandleMessage(string message);
    }
}
