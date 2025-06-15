using Boilerplate.Beta.Core.Infrastructure.Auth.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Boilerplate.Beta.Core.Infrastructure.Auth
{
    public class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenValidationService _tokenValidationService;

        public ActiveUserHandler(
            IHttpContextAccessor httpContextAccessor,
            ITokenValidationService tokenValidationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenValidationService = tokenValidationService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ActiveUserRequirement requirement)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(token) || !token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var accessToken = token["Bearer ".Length..].Trim();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return;
            }

            var isActive = await _tokenValidationService.ValidateTokenActiveAsync(accessToken);
            if (isActive)
            {
                context.Succeed(requirement);
            }
        }
    }
}