using Microsoft.AspNetCore.Hosting;
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
                .ConfigureAppSettings()
                .ConfigureWebHostDefaults(webBuilder =>
                {
					var builder = webBuilder.UseStartup<Startup>();
					builder.UseUrls(GetAppUrl());
				});

		private static string GetAppUrl()
		{
			var url = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
			return url;
		}
	}
}
