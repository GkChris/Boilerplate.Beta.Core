using Boilerplate.Beta.Core.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class AuthExtension
	{
        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection("AuthSettings");
            var authOptions = authSettings.Get<AuthSettings>();

            services.Configure<AuthSettings>(authSettings);

            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = authOptions.Authority;
                    options.Audience = authOptions.Audience;
                    options.RequireHttpsMetadata = authOptions.RequireHttpsMetadata;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = authOptions.ValidateIssuer,
                        ValidateAudience = authOptions.ValidateAudience,
                        ValidateLifetime = authOptions.ValidateLifetime,
                        ValidateIssuerSigningKey = authOptions.ValidateIssuerSigningKey,
                        RoleClaimType = authOptions.RoleClaim
                    };
                });

            services.AddAuthorization();
        }
    }
}