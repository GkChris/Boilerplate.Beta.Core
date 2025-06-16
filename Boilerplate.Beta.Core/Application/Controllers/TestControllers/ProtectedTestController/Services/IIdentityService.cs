using System.Text.Json;

namespace Boilerplate.Beta.Core.Application.Controllers.TestControllers.ProtectedTest.Services
{
    public interface IIdentityService
    {
        Task<string?> LoginAsync(string username, string password);
        Task<string?> LoginWithSocialAsync(string code, string redirectUri);
        Task<bool> LogoutAsync();
        Task<JsonDocument?> FetchUserInfoAsync(string token);
    }
}
