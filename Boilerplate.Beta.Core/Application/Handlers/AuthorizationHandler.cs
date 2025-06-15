using Boilerplate.Beta.Core.Application.Services.Abstractions.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Boilerplate.Beta.Core.Application.Handlers
{
    public class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public ActiveUserHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUserRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var token = httpContext?.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(token) || !token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return; // no token, nothing to validate
            }

            var accessToken = token.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return; // malformed token
            }

            var isActive = await _identityService.ValidateTokenActiveAsync(accessToken);

            if (isActive)
            {
                context.Succeed(requirement);
            }
        }
    }

    public class ActiveUserRequirement : IAuthorizationRequirement
        {
            // You can optionally add parameters if needed
        }

}
