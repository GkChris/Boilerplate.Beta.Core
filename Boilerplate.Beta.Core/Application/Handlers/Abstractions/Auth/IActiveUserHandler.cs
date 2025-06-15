using Boilerplate.Beta.Core.Application.Handlers.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Boilerplate.Beta.Core.Application.Handlers.Abstractions.Auth
{
    public interface IActiveUserHandler
    {
        Task HandleAsync(AuthorizationHandlerContext context, ActiveUserRequirement requirement);
    }
}
