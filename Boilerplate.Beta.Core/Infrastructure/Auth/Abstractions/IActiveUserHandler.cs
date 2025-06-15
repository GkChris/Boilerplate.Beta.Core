using Microsoft.AspNetCore.Authorization;

namespace Boilerplate.Beta.Core.Infrastructure.Auth.Abstractions
{
    public interface IActiveUserHandler
    {
        Task HandleAsync(AuthorizationHandlerContext context, ActiveUserRequirement requirement);
    }
}
