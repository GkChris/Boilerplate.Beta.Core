namespace Boilerplate.Beta.Core.Infrastructure.Configuration
{
    public class AuthSettings
    {
        // Core URLs
        public string Authority { get; set; }
        public string Audience { get; set; }

        // Client credentials
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        // Validation strategy    
        public bool RequireHttpsMetadata { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }

        // Claims mapping
        public string RoleClaim { get; set; }

        // Additional 
        public string ClientName { get; set; }

        // Conditions
        public bool AcceptTokenFromCookie { get; set; }
        public bool AcceptTokenFromHeader { get; set; }

        // Testing 
        public string TokenEndpoint { get; set; } //Testing
        public string UserInfoEndpoint { get; set; } //Testing
        public string IntrospectEndpoint { get; set; } //Testing
        public string LogoutEndpoint { get; set; } //Testing
    }
}