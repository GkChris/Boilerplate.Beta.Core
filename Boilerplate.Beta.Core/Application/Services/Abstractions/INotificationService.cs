namespace Boilerplate.Beta.Core.Application.Services.Abstractions
{
    public interface INotificationService
    {
        Task SendMessageToAllClients(string message);
    }
}
