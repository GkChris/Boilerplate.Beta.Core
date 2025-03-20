using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Handlers
{
    public class WebSocketMessageHandler : IWebSocketMessageHandler
    {
        private readonly ILogger<WebSocketMessageHandler> _logger;

        public WebSocketMessageHandler(ILogger<WebSocketMessageHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleMessageAsync(string clientId, string message)
        {
            _logger.LogInformation("Handling chat message from {ClientId}: {Message}", clientId, message);

            await Task.CompletedTask;
        }

        public bool CanHandleMessage(string message)
        {
            return message.StartsWith("chat:", StringComparison.OrdinalIgnoreCase);
        }
    }
}
