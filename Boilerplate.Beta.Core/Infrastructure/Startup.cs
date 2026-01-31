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
            services.AddCoreDependencies();
            services.AddSwaggerConfiguration(Configuration);
			services.AddSignalRBus();
			services.AddKafkaBus(Configuration);
			services.AddAuth(Configuration);
            services.AddHttpClients(Configuration);
            services.AddHttpContextAccessor();
			services.AddConfiguredCors(Configuration);
			services.AddRateLimiting(Configuration);
        }

        // Configure the HTTP request pipeline here
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
            var isDevelopment = env.IsEnvironment(EnvInternalNames.LocalDevelopment) || env.IsEnvironment(EnvInternalNames.DockerDevelopment);
            if (isDevelopment)
			{
				app.UseDeveloperExceptionPage();
				app.UseSwaggerUIConfiguration(Configuration);
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

            app.UseHttpsRedirection();
			app.UseRouting();

            app.UseCors("DefaultCorsPolicy");

            var infraOptions = app.ApplicationServices.GetRequiredService<IOptions<InfrastructureSettings>>();
            
            if (infraOptions.Value.EnableRateLimiting)
            {
                app.UseRateLimitingMiddleware();
            }

            app.UseTimeoutMiddleware(infraOptions.Value.RequestTimeoutSeconds);

            app.UseMiddleware<ErrorHandlingMiddleware>();
			app.UseMiddleware<CustomLoggingMiddleware>();

			app.UseAuthentication();
            app.UseAuthorization();

			app.UseSignalREndpoints();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

            if (infraOptions.Value.AutoApplyMigrations)
			{
				app.ApplicationServices.UseAutoMigrations();
			}
		}
	}
}