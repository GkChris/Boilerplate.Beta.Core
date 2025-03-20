using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets.Abstractions;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets
{
    public class WebSocketManager : IWebSocketManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _connections = new();
        private readonly ILogger<WebSocketManager> _logger;

        public WebSocketManager(ILogger<WebSocketManager> logger)
        {
            _logger = logger;
        }

        public async Task AddConnection(string clientId, WebSocket socket)
        {
            _connections[clientId] = socket;
            _logger.LogInformation("WebSocket connected: {ClientId}", clientId);
        }

        public async Task RemoveConnection(string clientId)
        {
            if (_connections.TryRemove(clientId, out var socket))
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
                _logger.LogInformation("WebSocket disconnected: {ClientId}", clientId);
            }
        }

        public async Task SendMessageToClient(string clientId, string message)
        {
            if (_connections.TryGetValue(clientId, out var socket) && socket.State == WebSocketState.Open)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task SendMessageToAllClients(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            foreach (var (clientId, socket) in _connections)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        public IReadOnlyCollection<string> GetConnectedClients() => _connections.Keys.ToList();
    }
}