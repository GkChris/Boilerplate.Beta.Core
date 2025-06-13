namespace Boilerplate.Beta.Core.Infrastructure.Configuration
{
    public class AuthSettings
    {
        // Core URLs
        public string Authority { get; set; }              
        public string Audience { get; set; }               
        public string TokenEndpoint { get; set; }
        public string IntrospectionEndpoint { get; set; }

        // Client credentials (if doing introspection or token requests)
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        // Validation strategy    
        public bool RequireHttpsMetadata { get; set; } = true;
        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateLifetime { get; set; } = true;
        public bool ValidateIssuerSigningKey { get; set; } = true;
        public bool UseTokenIntrospection { get; set; }

        // Claims mapping
        public string RoleClaim { get; set; } = "roles";
    }
}