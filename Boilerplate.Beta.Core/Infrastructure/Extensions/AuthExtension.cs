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
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration["Jwt:Authority"]; 
                    options.Audience = configuration["Jwt:Audience"];   
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RoleClaimType = "roles"
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"[Auth Failed] {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine($"[Token Validated] {context.SecurityToken}");
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }
    }
}