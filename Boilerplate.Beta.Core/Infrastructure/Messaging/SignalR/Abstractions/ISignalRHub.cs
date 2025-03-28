namespace Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets.Abstractions
{
	public interface IChatHub
	{
		Task SendMessageToClient(string clientId, string message);
		Task SendMessageToAllClients(string message);
		Task OnConnectedAsync();
		Task OnDisconnectedAsync(Exception? exception);
	}
}