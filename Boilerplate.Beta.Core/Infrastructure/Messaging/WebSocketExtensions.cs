using Boilerplate.Beta.Core.Application.Messaging.WebSockets;
using Boilerplate.Beta.Core.Application.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging
{
	public static class WebSocketExtensions
	{
		public static void AddWebSocketServices(this IServiceCollection services)
		{
			services.AddSingleton<IWebSocketManager, WebSocketManager>();
		}

		public static void UseWebSocketMiddleware(this IApplicationBuilder app)
		{
			app.UseWebSockets();
			app.UseMiddleware<WebSocketMiddleware>();
		}
	}
}
