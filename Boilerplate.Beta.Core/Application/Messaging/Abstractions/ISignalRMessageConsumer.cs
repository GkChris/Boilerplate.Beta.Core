namespace Boilerplate.Beta.Core.Application.Handlers.Abstractions
{
    public interface ISignalRMessageConsumer
    {
        Task HandleMessageAsync(string message);

        bool CanHandleMessage(string message);
    }
}
