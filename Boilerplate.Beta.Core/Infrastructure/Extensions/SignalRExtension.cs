using Boilerplate.Beta.Core.Application.Handlers;
using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Boilerplate.Beta.Core.Application.Services.Abstractions.SignalR;
using Boilerplate.Beta.Core.Application.Services.SignalR;
using Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class SignalRExtension
	{
		public static void AddSignalRBus(this IServiceCollection services)
		{
			services.AddSignalRPublisher();
			services.AddSignalRConsumer();
		}

		public static void AddSignalRPublisher(this IServiceCollection services)
		{
			services.AddSignalR();
			services.AddSingleton<ChatHub>();
			services.AddSingleton<ISignalRPublisherService, SignalRPublisherService>();
		}

		public static void AddSignalRConsumer(this IServiceCollection services)
		{
			services.AddSignalR();
			services.AddSingleton<ISignalRMessageHandler, SignalRMessageHandler>();
			services.AddHostedService<SignalRClientBackgroundService>();
		}

		public static void UseSignalREndpoints(this IApplicationBuilder app)
		{
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHub<ChatHub>("/chatHub");
			});
		}
	}
}