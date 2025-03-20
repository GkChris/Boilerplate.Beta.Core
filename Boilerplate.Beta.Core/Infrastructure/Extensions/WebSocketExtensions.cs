using Boilerplate.Beta.Core.Application.Handlers;
using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Boilerplate.Beta.Core.Application.Services;
using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets;
using Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class WebSocketExtensions
    {
        public static void AddWebSocketServices(this IServiceCollection services)
        {
            services.AddSingleton<IWebSocketManager, WebSocketManager>();
            services.AddSingleton<WebSocketHub>();
            services.AddScoped<IWebSocketPublisherService, WebSocketPublisherService>();
            services.AddSingleton<IWebSocketMessageHandler, WebSocketMessageHandler>();
        }

        public static void UseWebSocketMiddleware(this IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.UseMiddleware<WebSocketMiddleware>();
        }
    }
}