using System.Net.WebSockets;

namespace Boilerplate.Beta.Core.Application.Messaging.WebSockets
{
	public interface IWebSocketManager
	{
		Task AddConnection(WebSocket socket);
		Task RemoveConnection(WebSocket socket);
		Task SendMessageToAllClients(string message);
		Task SendMessageToClient(WebSocket socket, string message);
	}

}
