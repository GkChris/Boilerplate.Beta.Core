using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
	public static class AuthExtension
	{
		public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["Jwt:Authority"]; 
                    options.Audience = configuration["Jwt:Audience"];
                    options.RequireHttpsMetadata = !bool.TryParse(configuration["Jwt:RequireHttpsMetadata"], out var requireHttps) || requireHttps; // Should be false only in Dev

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RoleClaimType = "roles"
                    };
                });

            services.AddAuthorization();
        }
    }
}