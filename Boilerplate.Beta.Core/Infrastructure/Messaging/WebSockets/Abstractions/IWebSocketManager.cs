using System.Net.WebSockets;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets.Abstractions
{
    public interface IWebSocketManager
    {
        Task AddConnection(string clientId, WebSocket socket);
        Task RemoveConnection(string clientId);
        Task SendMessageToClient(string clientId, string message);
        Task SendMessageToAllClients(string message);
    }
}