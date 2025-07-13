using Boilerplate.Beta.Core.Application.Controllers.TestControllers.ProtectedTest.Services;
using Boilerplate.Beta.Core.Infrastructure.Auth;
using Boilerplate.Beta.Core.Infrastructure.Auth.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

            bool acceptTokenFromCookie = authOptions.AcceptTokenFromCookie;
            bool acceptTokenFromHeader = authOptions.AcceptTokenFromHeader;

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

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            string token = null;

                            if (acceptTokenFromCookie)
                            {
                                token = context.Request.Cookies["app.at"];
                            }

                            if (string.IsNullOrEmpty(token) && acceptTokenFromHeader)
                            {
                                var authHeader = context.Request.Headers["Authorization"].ToString();
                                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                                {
                                    token = authHeader.Substring("Bearer ".Length).Trim();
                                }
                            }

                            context.Token = token;
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ActiveUser", policy =>
                    policy.RequireAuthenticatedUser()
                          .AddRequirements(new ActiveUserRequirement()));
            });

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAuthorizationHandler, ActiveUserHandler>();
            services.AddScoped<ITokenValidationService, TokenValidationService>();
        }
    }
}