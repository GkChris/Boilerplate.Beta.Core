namespace Boilerplate.Beta.Core.Application.Services.Abstractions.Auth
{
    public interface IIdentityService
    {
        Task<string?> LoginAsync(string username, string password);
        Task<bool> ValidateTokenAsync(string token);
    }
}
