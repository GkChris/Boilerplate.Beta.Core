using System.Text.Json;

namespace Boilerplate.Beta.Core.Application.Services.Abstractions
{
    public interface IIdentityService
    {
        Task<string?> LoginAsync(string username, string password);
        Task<JsonDocument?> FetchUserInfoAsync(string token);
    }
}
