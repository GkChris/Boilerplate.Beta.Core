using Boilerplate.Beta.Core.Application.Middlewares;
using Boilerplate.Beta.Core.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Boilerplate.Beta.Core.Infrastructure
{
	public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructureConfiguration(Configuration);
            services.AddDatabaseServices(Configuration);
            services.AddApiControllers();
            services.AddApplicationServices();
            services.AddSwaggerConfiguration();
			services.AddSignalRBus();
			services.AddKafkaBus(Configuration);
		}

		// Configure the HTTP request pipeline here
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwaggerUIConfiguration();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseMiddleware<CustomLoggingMiddleware>();
			app.UseMiddleware<ErrorHandlingMiddleware>();

			app.UseAuthorization();
			app.UseSignalREndpoints();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			var infrastructureSettings = Configuration
				.GetSection("InfrastructureSettings")
				.Get<InfrastructureSettings>() ?? new InfrastructureSettings();

			if (infrastructureSettings.AutoApplyMigrations)
			{
				app.ApplicationServices.UseAutoMigrations();
			}
		}
	}
}