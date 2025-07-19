using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Boilerplate.Beta.Core.Infrastructure
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.Sources.Clear(); // Optional: clear default sources to control order

                    config.AddJsonFile("Infrastructure/appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"Infrastructure/appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                          .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    var url = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
                    if (!string.IsNullOrEmpty(url))
                    {
                        webBuilder.UseUrls(url);
                    }
                });
    }
}
