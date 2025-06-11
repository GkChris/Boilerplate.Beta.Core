namespace Boilerplate.Beta.Core.Infrastructure
{
	public class JwtSettings
	{
		public string Authority { get; set; }
		public string Audience { get; set; }
		public bool RequireHttpsMetadata { get; set; }
		public string RoleClaim { get; set; }
    }
}