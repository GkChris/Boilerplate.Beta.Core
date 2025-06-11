using Boilerplate.Beta.Core.Infrastructure;
using Boilerplate.Beta.Core.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class ConfigurationExtensions
{
    public static IHostBuilder ConfigureAppSettings(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration((context, config) =>
        {
            var env = context.HostingEnvironment;

            config.SetBasePath(env.ContentRootPath)
                  .AddJsonFile("Infrastructure/appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"Infrastructure/appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        });
    }

    public static void AddInfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        services.Configure<SwaggerSettings>(configuration.GetSection("Swagger"));
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
        services.Configure<MetaDataSettings>(configuration.GetSection("MetaData"));
        services.Configure<LoggingSettings>(configuration.GetSection("Logging"));
        services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
		services.Configure<InfrastructureSettings>(configuration.GetSection("InfrastructureSettings"));
        services.Configure<FusionAuthSettings>(configuration.GetSection("FusionAuth"));
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
    }
}