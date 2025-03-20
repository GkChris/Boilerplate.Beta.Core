namespace Boilerplate.Beta.Core.Application.Services
{
	public interface INotificationService
	{
		Task SendMessageToAllClients(string message);
	}
}
