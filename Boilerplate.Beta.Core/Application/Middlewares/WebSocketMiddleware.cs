using Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets;
using Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Middlewares
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketManager _webSocketManager;
        private readonly WebSocketHub _webSocketHub;
        private readonly ILogger<WebSocketMiddleware> _logger;

        public WebSocketMiddleware(RequestDelegate next, IWebSocketManager webSocketManager, WebSocketHub webSocketHub, ILogger<WebSocketMiddleware> logger)
        {
            _next = next;
            _webSocketManager = webSocketManager;
            _webSocketHub = webSocketHub;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
            {
                string clientId = context.Request.Query["clientId"]; // Replace with auth later
                if (string.IsNullOrEmpty(clientId))
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                var socket = await context.WebSockets.AcceptWebSocketAsync();
                await _webSocketManager.AddConnection(clientId, socket);
                await _webSocketHub.HandleClientMessages(clientId, socket);
            }
            else
            {
                await _next(context);
            }
        }
    }
}