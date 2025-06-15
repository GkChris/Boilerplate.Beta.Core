namespace Boilerplate.Beta.Core.Infrastructure.Auth.Abstractions
{
    public interface ITokenValidationService
    {
        Task<bool> ValidateTokenActiveAsync(string token);
    }
}
