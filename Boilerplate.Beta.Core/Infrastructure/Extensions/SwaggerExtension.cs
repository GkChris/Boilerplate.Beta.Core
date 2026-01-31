using Boilerplate.Beta.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
	public static class SwaggerExtension
	{
		public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			var settings = configuration.GetSection("Swagger").Get<SwaggerSettings>();
			if (settings is null)
			{
				throw new InvalidOperationException("Swagger settings are missing. Ensure 'Swagger' section exists in configuration.");
			}

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = settings.Title,
					Version = settings.Version,
					Description = settings.Description,
					Contact = settings.Contact is null
						? null
						: new OpenApiContact
						{
							Name = settings.Contact.Name,
							Email = settings.Contact.Email,
							Url = settings.Contact.Url
						}
				});

				c.EnableAnnotations();

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter 'Bearer' [space] and then your valid JWT token.\nExample: \"Bearer eyJhbGciOi...\""
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
			});
		}

		public static void UseSwaggerUIConfiguration(this IApplicationBuilder app, IConfiguration configuration)
		{
			var settings = configuration.GetSection("Swagger").Get<SwaggerSettings>();
			if (settings is null)
			{
				throw new InvalidOperationException("Swagger settings are missing. Ensure 'Swagger' section exists in configuration.");
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{settings.Title} {settings.Version}");
				c.RoutePrefix = string.Empty;
			});
		}
	}
}