namespace Boilerplate.Beta.Core.Infrastructure.Configuration
{
    public class AuthSettings
    {
        // Core URLs
        public string Authority { get; set; }              
        public string Audience { get; set; }               
        public string TokenEndpoint { get; set; }
        public string UserInfoEndpoint { get; set; }
        public string IntrospectEndpoint { get; set; }
        public string LogoutEndpoint { get; set; }

        // Client credentials
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        // Validation strategy    
        public bool RequireHttpsMetadata { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }

        // Post-validation strategy
        public bool EnablePostValidation { get; set; }

        // Claims mapping
        public string RoleClaim { get; set; }

        // Additional 
        public string ClientName { get; set; }
    }
}