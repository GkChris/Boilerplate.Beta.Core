using Boilerplate.Beta.Core.Application.Handlers;
using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR;
using Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR.Abstractions;
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
			services.AddSingleton<ISignalRPublisher, SignalRPublisher>();
			services.AddSingleton<ISignalRMessagePublisher, SignalRMessagePublisher>();
		}

		public static void AddSignalRConsumer(this IServiceCollection services)
		{
			services.AddSignalR();
			services.AddSingleton<ISignalRMessageConsumer, SignalRMessageConsumer>();
			services.AddHostedService<SignalRClientBackgroundService>();
		}

		public static void UseSignalREndpoints(this IApplicationBuilder app)
		{
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHub<SignalRHub>("/chatHub");
			});
		}
	}
}