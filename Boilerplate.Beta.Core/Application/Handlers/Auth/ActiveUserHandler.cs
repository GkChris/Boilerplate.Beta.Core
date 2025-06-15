using Boilerplate.Beta.Core.Application.Services.Abstractions.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Boilerplate.Beta.Core.Application.Handlers.Auth
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

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ActiveUserRequirement requirement)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(token) || !token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return;

            var accessToken = token["Bearer ".Length..].Trim();

            if (string.IsNullOrWhiteSpace(accessToken))
                return;

            var isActive = await _identityService.ValidateTokenActiveAsync(accessToken);

            if (isActive)
                context.Succeed(requirement);
        }
    }
}