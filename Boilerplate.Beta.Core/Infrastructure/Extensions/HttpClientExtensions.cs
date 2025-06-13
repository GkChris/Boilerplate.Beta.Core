using Boilerplate.Beta.Core.Application.Services.Abstractions.Auth;
using Boilerplate.Beta.Core.Application.Services.Auth;
using Boilerplate.Beta.Core.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            // FusionAuth
            var fusionSettings = configuration.GetSection("FusionAuth").Get<FusionAuthSettings>();
            services.Configure<FusionAuthSettings>(configuration.GetSection("FusionAuth"));

            services.AddHttpClient<IIdentityService, IdentityService>(client =>
            {
                client.BaseAddress = new Uri(fusionSettings.BaseUrl);
            });

            // Add more clients here...

            return services;
        }
    }
}
