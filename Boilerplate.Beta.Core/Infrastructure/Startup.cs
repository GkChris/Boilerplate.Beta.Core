using System.Text.Json.Serialization;
using Boilerplate.Beta.Core.Data;
using Boilerplate.Beta.Core.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Boilerplate.Beta.Core.Infrastructure
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        private readonly string ApplicationName;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ApplicationName = Configuration.GetValue<string>("MetaData:ApplicationName") ?? string.Empty;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddCoreServices();
            services.AddSwaggerConfiguration();
			services.AddWebSocketServices();
			services.AddKafka(Configuration);
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

			// Common middleware for web applications
			app.UseWebSocketMiddleware();
            app.UseKafka();

			app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
			});
        }
    }
}