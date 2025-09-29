namespace Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR.Abstractions
{
	public interface ISignalRPublisher
	{
		Task SendMessageToClient(string clientId, string message);
		Task SendMessageToAllClients(string message);
	}
}