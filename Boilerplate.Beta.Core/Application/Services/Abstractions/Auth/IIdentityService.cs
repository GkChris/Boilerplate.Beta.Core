using System.Text.Json;

namespace Boilerplate.Beta.Core.Application.Services.Abstractions.Auth
{
    public interface IIdentityService
    {
        Task<string?> LoginAsync(string username, string password);
        Task<JsonDocument?> FetchUserInfoAsync(string token);
        Task<bool> ValidateTokenActiveAsync(string token);
    }
}
