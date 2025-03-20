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

        public async Task AddConnection(WebSocket socket)
        {
            var socketId = Guid.NewGuid().ToString();
            _connections.TryAdd(socketId, socket);

            _logger.LogInformation("WebSocket connection added. ID: {SocketId}", socketId);

            await ListenToClientMessages(socket, socketId);
        }

        public async Task RemoveConnection(WebSocket socket)
        {
            var socketId = _connections.FirstOrDefault(c => c.Value == socket).Key;
            if (socketId != null)
            {
                _connections.TryRemove(socketId, out _);
                _logger.LogInformation("WebSocket connection removed. ID: {SocketId}", socketId);

                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the server", CancellationToken.None);
            }
        }

        public async Task SendMessageToAllClients(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            _logger.LogInformation("Broadcasting message to all clients: {Message}", message);

            foreach (var connection in _connections.Values)
            {
                if (connection.State == WebSocketState.Open)
                {
                    await connection.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        public async Task SendMessageToClient(WebSocket socket, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                _logger.LogInformation("Sent message to client: {Message}", message);
            }
        }

        private async Task ListenToClientMessages(WebSocket socket, string socketId)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.LogInformation("Client requested to close WebSocket connection. ID: {SocketId}", socketId);
                    await RemoveConnection(socket);
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    _logger.LogInformation("Received message from {SocketId}: {Message}", socketId, message);
                }
            }
        }
    }
}
