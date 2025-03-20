using Boilerplate.Beta.Core.Application.Messaging.WebSockets;
using Microsoft.AspNetCore.Http;


namespace Boilerplate.Beta.Core.Application.Middlewares
{
	public class WebSocketMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IWebSocketManager _webSocketManager;

		public WebSocketMiddleware(RequestDelegate next, IWebSocketManager webSocketManager)
		{
			_next = next;
			_webSocketManager = webSocketManager;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
			{
				var socket = await context.WebSockets.AcceptWebSocketAsync();
				await _webSocketManager.AddConnection(socket);
			}
			else
			{
				await _next(context);
			}
		}
	}

}
