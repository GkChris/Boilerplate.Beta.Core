using Boilerplate.Beta.Core.Application.Middlewares;
using Boilerplate.Beta.Core.Application.Shared.Constants;
using Boilerplate.Beta.Core.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
			services.AddAuth(Configuration);
            services.AddHttpClients(Configuration);
            services.AddHttpContextAccessor();
			services.AddConfiguredCors(Configuration);
        }

        // Configure the HTTP request pipeline here
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
            var isDevelopment = env.IsEnvironment(EnvInternalNames.LocalDevelopment) || env.IsEnvironment(EnvInternalNames.DockerDevelopment);
            if (isDevelopment)
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

            app.UseCors("DefaultCorsPolicy");

            app.UseMiddleware<ErrorHandlingMiddleware>();
			app.UseMiddleware<CustomLoggingMiddleware>();

			app.UseAuthentication();
            app.UseAuthorization();

			app.UseSignalREndpoints();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

            var infraOptions = app.ApplicationServices.GetRequiredService<IOptions<InfrastructureSettings>>();
            if (infraOptions.Value.AutoApplyMigrations)
			{
				app.ApplicationServices.UseAutoMigrations();
			}
		}
	}
}