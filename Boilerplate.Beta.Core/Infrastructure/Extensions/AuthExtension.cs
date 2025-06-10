using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
	public static class AuthExtension
	{
		public static void AddAuthenticationWithJwtOptions(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication("Bearer")
				.AddJwtBearer("Bearer", options =>
				{
					options.Authority = configuration["Jwt:Authority"];
					options.Audience = configuration["Jwt:Audience"];
					options.RequireHttpsMetadata = bool.Parse(configuration["Jwt:RequireHttpsMetadata"] ?? "false");

					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateAudience = true,
						ValidateIssuer = true,
						ValidateIssuerSigningKey = true,
						ValidateLifetime = true
					};
				});
		}
	}
}
