using Boilerplate.Beta.Core.Application.Messaging.WebSockets;
using Boilerplate.Beta.Core.Application.Middlewares;
using Boilerplate.Beta.Core.Infrastructure.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class WebSocketExtensions
    {
        public static void AddWebSocketServices(this IServiceCollection services)
        {
            services.AddSignalR();
			services.AddSingleton<IWebSocketManager, WebSocketManager>();
        }

        public static void UseWebSocketMiddleware(this IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.UseMiddleware<WebSocketMiddleware>();
		}

		public static void MapMessagingEndpoints(this IEndpointRouteBuilder endpoints)
		{
			endpoints.MapHub<WebSocketHub>("/websocketHub");
		}
	}
}