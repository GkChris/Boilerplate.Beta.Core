using System.Threading.Tasks;
using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets.Abstractions;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Services
{
    public class WebSocketPublisherService : IWebSocketPublisherService
    {
        private readonly IWebSocketManager _webSocketManager;
        private readonly ILogger<WebSocketPublisherService> _logger;

        public WebSocketPublisherService(IWebSocketManager webSocketManager, ILogger<WebSocketPublisherService> logger)
        {
            _webSocketManager = webSocketManager;
            _logger = logger;
        }

        public async Task SendMessageToAllAsync(string message)
        {
            _logger.LogInformation("Sending message to all WebSocket clients: {Message}", message);
            await _webSocketManager.SendMessageToAllClients(message);
        }

        public async Task SendMessageToClientAsync(string clientId, string message)
        {
            _logger.LogInformation("Sending message to WebSocket client {ClientId}: {Message}", clientId, message);
            await _webSocketManager.SendMessageToClient(clientId, message);
        }
    }
}