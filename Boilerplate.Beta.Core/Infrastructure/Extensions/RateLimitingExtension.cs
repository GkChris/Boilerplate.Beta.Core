using System.Threading.RateLimiting;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class RateLimitingExtension
    {
        /// <summary>
        /// Adds rate limiting with three policies configured from appsettings.json
        /// All values are dynamic - change them in config without code changes
        /// Default policy is automatically applied to ALL endpoints
        /// </summary>
        public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            var infraSettings = configuration.GetSection("InfrastructureSettings").Get<InfrastructureSettings>();
            
            if (infraSettings == null)
            {
                throw new InvalidOperationException("InfrastructureSettings not configured");
            }

            services.AddRateLimiter(options =>
            {
                AddPolicy(options, "default", infraSettings.DefaultPolicy);
                AddPolicy(options, "strict", infraSettings.StrictPolicy);
                AddPolicy(options, "lenient", infraSettings.LenientPolicy);

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    return RateLimitPartition.GetSlidingWindowLimiter<string>("default",
                        _ => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = infraSettings.DefaultPolicy.PermitLimit,
                            Window = TimeSpan.FromMinutes(infraSettings.DefaultPolicy.WindowMinutes),
                            SegmentsPerWindow = infraSettings.DefaultPolicy.SegmentsPerWindow,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = infraSettings.DefaultPolicy.QueueLimit
                        });
                });

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    context.HttpContext.Response.ContentType = "application/json";

                    var retryAfter = "60"; 
                    var policy = GetCurrentPolicy(context.HttpContext);
                    
                    if (policy != null)
                    {
                        retryAfter = (policy.WindowMinutes * 60).ToString();
                    }

                    var response = new
                    {
                        error = new
                        {
                            type = "TooManyRequests",
                            message = "Rate limit exceeded.",
                            friendlyMessage = "You have made too many requests. Please try again later.",
                            retryAfter = int.Parse(retryAfter),
                            resetTime = DateTime.UtcNow.AddSeconds(int.Parse(retryAfter)),
                            policy = GetCurrentPolicyName(context.HttpContext)
                        }
                    };

                    context.HttpContext.Response.Headers["Retry-After"] = retryAfter;
                    context.HttpContext.Response.Headers["X-RateLimit-Policy"] = GetCurrentPolicyName(context.HttpContext) ?? "default";
                    
                    var json = JsonSerializer.Serialize(response);
                    await context.HttpContext.Response.WriteAsync(json, cancellationToken);
                };
            });

            return services;
        }

        /// <summary>
        /// Helper method to add a policy with all its settings
        /// </summary>
        private static void AddPolicy(RateLimiterOptions options, string policyName, RateLimitingPolicy policy)
        {
            options.AddSlidingWindowLimiter(policyName, limiterOptions =>
            {
                limiterOptions.PermitLimit = policy.PermitLimit;
                limiterOptions.Window = TimeSpan.FromMinutes(policy.WindowMinutes);
                limiterOptions.SegmentsPerWindow = policy.SegmentsPerWindow;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = policy.QueueLimit;
            });
        }

        /// <summary>
        /// Extract current policy from endpoint metadata
        /// </summary>
        private static RateLimitingPolicy GetCurrentPolicy(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var policyName = endpoint?.Metadata
                .OfType<Microsoft.AspNetCore.RateLimiting.EnableRateLimitingAttribute>()
                .FirstOrDefault()?.PolicyName;
            
            return policyName switch
            {
                "strict" => new RateLimitingPolicy { WindowMinutes = 1 },
                "default" => new RateLimitingPolicy { WindowMinutes = 1 },
                "lenient" => new RateLimitingPolicy { WindowMinutes = 1 },
                _ => null
            };
        }

        /// <summary>
        /// Get the policy name for logging (defaults to "default" if no attribute specified)
        /// </summary>
        private static string GetCurrentPolicyName(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            return endpoint?.Metadata
                .OfType<Microsoft.AspNetCore.RateLimiting.EnableRateLimitingAttribute>()
                .FirstOrDefault()?.PolicyName ?? "default";
        }

        /// <summary>
        /// Applies rate limiting middleware to the pipeline.
        /// Must be called after UseRouting() for endpoint metadata to be available.
        /// </summary>
        public static IApplicationBuilder UseRateLimitingMiddleware(this IApplicationBuilder app)
        {
            app.UseRateLimiter();
            return app;
        }

        /// <summary>
        /// Applies request timeout protection to the pipeline.
        /// Cancels any request that exceeds the configured timeout (default: 30 seconds).
        /// Returns 408 Request Timeout on timeout.
        /// </summary>
        public static IApplicationBuilder UseTimeoutMiddleware(this IApplicationBuilder app, int timeoutSeconds = 30)
        {
            app.Use(async (context, next) =>
            {
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted, timeoutCts.Token);

                try
                {
                    context.RequestAborted = combinedCts.Token;
                    await next(context);
                }
                catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested && !context.RequestAborted.IsCancellationRequested)
                {
                    context.Response.StatusCode = 408;
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        error = new
                        {
                            type = "RequestTimeout",
                            message = "The request timed out.",
                            friendlyMessage = "Your request took too long to process. Please try again."
                        }
                    };

                    var json = JsonSerializer.Serialize(response);
                    await context.Response.WriteAsync(json);
                }
            });

            return app;
        }
    }
}
