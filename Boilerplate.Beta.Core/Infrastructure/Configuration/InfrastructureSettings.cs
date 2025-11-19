namespace Boilerplate.Beta.Core.Infrastructure
{
    public class InfrastructureSettings
    {
        public bool AutoApplyMigrations { get; set; }
		public bool EnableCustomLoggingMiddleware { get; set; }
        public bool LogExceptionStackTrace { get; set; }
		public bool EnableRateLimiting { get; set; }
		public int RequestTimeoutSeconds { get; set; } = 30;
		public RateLimitingPolicy DefaultPolicy { get; set; } = new();
		public RateLimitingPolicy StrictPolicy { get; set; } = new();
		public RateLimitingPolicy LenientPolicy { get; set; } = new();
		public string DefaultPolicyName { get; set; } = "default";
	}

	public class RateLimitingPolicy
	{
		/// <summary>
		/// Maximum requests allowed in the time window
		/// </summary>
		public int PermitLimit { get; set; }

		/// <summary>
		/// Time window in minutes
		/// </summary>
		public int WindowMinutes { get; set; }

		/// <summary>
		/// Number of segments to divide the window into (for sliding window)
		/// </summary>
		public int SegmentsPerWindow { get; set; } = 4;

		/// <summary>
		/// Maximum requests that can queue before rejection
		/// </summary>
		public int QueueLimit { get; set; }

		/// <summary>
		/// Description for documentation
		/// </summary>
		public string Description { get; set; }
	}
}
