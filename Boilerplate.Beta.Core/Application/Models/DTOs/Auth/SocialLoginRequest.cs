namespace Boilerplate.Beta.Core.Application.Models.DTOs.Auth
{
    public class SocialLoginRequest
    {
        public string Code { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
    }
}
