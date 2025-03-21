using Boilerplate.Beta.Core.Application.Services;
using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
	public static class CoreServicesExtension
	{
		public static void AddCoreServices(this IServiceCollection services)
		{
			services.AddScoped<IEntityService, EntityService>();
		}
	}
}
