using Boilerplate.Beta.Core.Application.Handlers;
using Boilerplate.Beta.Core.Application.Services;
using Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
	public static class SignalRExtension
	{
		public static void AddSignalRServices(this IServiceCollection services)
		{
			services.AddSignalR();
			services.AddSingleton<ChatHub>();
			services.AddSingleton<SignalRPublisherService>();
			services.AddSingleton<SignalRMessageHandler>();
		}

		public static void UseSignalRMiddleware(this IApplicationBuilder app)
		{
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHub<ChatHub>("/chatHub");
			});
		}
	}
}