namespace Boilerplate.Beta.Core.Infrastructure
{
	public class Jwt
	{
		public string Authority { get; set; }
		public string Audience { get; set; }
		public bool RequireHttpsMetadata { get; set; }
	}
}