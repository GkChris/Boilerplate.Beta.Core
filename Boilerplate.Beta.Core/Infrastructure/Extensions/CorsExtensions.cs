using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class CorsExtensions
    {
        public static void AddConfiguredCors(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>();

            if (allowedOrigins == null || allowedOrigins.Length == 0)
            {
                var originsString = configuration["Cors:AllowedOrigins"];
                if (!string.IsNullOrWhiteSpace(originsString))
                {
                    allowedOrigins = originsString
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                }
            }

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy", policy =>
                {
                    policy
                        .WithOrigins(allowedOrigins ?? Array.Empty<string>())
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
    }
}