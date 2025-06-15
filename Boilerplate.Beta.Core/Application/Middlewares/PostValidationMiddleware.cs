using Boilerplate.Beta.Core.Application.Attributes;
using Boilerplate.Beta.Core.Application.Services.Abstractions.Auth;
using Boilerplate.Beta.Core.Infrastructure.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Boilerplate.Beta.Core.Application.Middlewares
{
    public class PostValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IIdentityService _identityService;
        private readonly AuthSettings _authSettings;

        public PostValidationMiddleware(
            RequestDelegate next,
            IIdentityService identityService,
            IOptions<AuthSettings> options)
        {
            _next = next;
            _identityService = identityService;
            _authSettings = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_authSettings.EnablePostValidation)
            {
                await _next(context);
                return;
            }

            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<RequiresPostValidationAttribute>() != null)
            {
                if (context.User?.Identity != null && context.User.Identity.IsAuthenticated)
                {
                    var authHeader = context.Request.Headers["Authorization"].ToString();

                    if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        var token = authHeader.Substring("Bearer ".Length).Trim();

                        if (!string.IsNullOrEmpty(token))
                        {
                            var isValid = await _identityService.ValidateTokenAsync(token);

                            if (!isValid)
                            {

                            }
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
