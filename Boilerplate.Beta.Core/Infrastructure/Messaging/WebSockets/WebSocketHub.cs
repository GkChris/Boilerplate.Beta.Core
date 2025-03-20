using System.Net.WebSockets;
using System.Text;
using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets.Abstractions;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets
{
    public class WebSocketHub : IWebSocketHub
    {
        private readonly IWebSocketManager _webSocketManager;
        private readonly ILogger<WebSocketHub> _logger;
        private readonly IEnumerable<IWebSocketMessageHandler> _messageHandlers; 

        public WebSocketHub(IWebSocketManager webSocketManager,
                            IEnumerable<IWebSocketMessageHandler> messageHandlers,
                            ILogger<WebSocketHub> logger)
        {
            _webSocketManager = webSocketManager;
            _messageHandlers = messageHandlers;
            _logger = logger;
        }

        public async Task HandleClientMessages(string clientId, WebSocket socket)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        _logger.LogInformation("Client {ClientId} disconnected", clientId);
                        await _webSocketManager.RemoveConnection(clientId);
                        break;
                    }

                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    _logger.LogInformation("Received from {ClientId}: {Message}", clientId, message);

                    var handler = _messageHandlers.FirstOrDefault(h => h.CanHandleMessage(message));

                    if (handler != null)
                    {
                        await handler.HandleMessageAsync(clientId, message);
                    }
                    else
                    {
                        _logger.LogWarning("No handler found for message: {Message}", message);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing WebSocket message for {ClientId}", clientId);
                    break;
                }
            }
        }
    }
}
