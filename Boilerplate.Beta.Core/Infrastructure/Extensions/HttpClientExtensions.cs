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
            var authSettings = configuration.GetSection("AuthSettings").Get<AuthSettings>();
            services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));

            services.AddHttpClient(authSettings.ClientName, client =>
            {
                client.BaseAddress = new Uri(authSettings.Authority);
            });

            // Add more clients here...

            return services;
        }
    }
}
