using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Diagnostics;

namespace Boilerplate.Beta.Core.Application.Controllers.TestControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateLimitingTestController : ControllerBase
    {
        /// <summary>
        /// Default policy endpoint (NO ATTRIBUTE NEEDED - applied automatically to ALL endpoints)
        /// Development: 10 req/min + 2-request queue
        /// Production: 100 req/min + 2-request queue
        /// 
        /// This demonstrates that the default policy is applied automatically.
        /// 
        /// HOW TO APPLY DIFFERENT POLICIES TO YOUR ENDPOINTS:
        /// 
        /// 1. NO ATTRIBUTE NEEDED - Default policy applied automatically
        /// [HttpGet("users")]
        /// public IActionResult GetUsers() { }  // Uses default policy automatically
        /// 
        /// 2. Override with Lenient policy (for public endpoints):
        /// [HttpGet("search")]
        /// [EnableRateLimiting("lenient")]
        /// public IActionResult Search() { }
        /// 
        /// 3. Override with Strict policy (for critical endpoints):
        /// [HttpPost("login")]
        /// [EnableRateLimiting("strict")]
        /// public IActionResult Login() { }
        /// </summary>
        [HttpGet("default")]
        public IActionResult DefaultTest()
        {
            return Ok(new 
            { 
                message = "Default policy (applied automatically to ALL endpoints)", 
                timestamp = DateTime.UtcNow, 
                policy = "default",
                note = "No [EnableRateLimiting] attribute needed - this policy is applied by default"
            });
        }

        /// <summary>
        /// Lenient policy endpoint - Override default with lenient (for public endpoints)
        /// Development: 50 req/min + 5-request queue
        /// Production: 500 req/min + 5-request queue
        /// Use for: Public endpoints, search, health checks
        /// </summary>
        [HttpGet("lenient")]
        [EnableRateLimiting("lenient")]
        public IActionResult LenientTest()
        {
            return Ok(new 
            { 
                message = "Lenient policy (override using [EnableRateLimiting(\"lenient\")])", 
                timestamp = DateTime.UtcNow, 
                policy = "lenient",
                useCase = "Public endpoints (search, status, health)"
            });
        }

        /// <summary>
        /// Strict policy endpoint - Override default with strict (for critical operations)
        /// Development: 5 req/min, NO queue
        /// Production: 30 req/min, NO queue
        /// Use for: Login, payments, sensitive operations
        /// </summary>
        [HttpGet("strict")]
        [EnableRateLimiting("strict")]
        public IActionResult StrictTest()
        {
            return Ok(new 
            { 
                message = "Strict policy (override using [EnableRateLimiting(\"strict\")])", 
                timestamp = DateTime.UtcNow, 
                policy = "strict",
                useCase = "Critical endpoints (login, payments, admin operations)"
            });
        }

        /// <summary>
        /// Slow endpoint for testing TIMEOUT (not rate limiting)
        /// Request will timeout after 30 seconds (configurable via RequestTimeoutSeconds in appsettings)
        /// Returns 408 Request Timeout
        /// 
        /// Uses DEFAULT policy (automatic) - this is a standard endpoint, not a public one
        /// 
        /// HOW TO TEST TIMEOUT ON SWAGGER:
        /// 1. Open Swagger UI
        /// 2. Click on this endpoint
        /// 3. Click "Try it out"
        /// 4. Click "Execute"
        /// 5. Wait ~30 seconds
        /// 6. You'll get 408 Request Timeout response
        /// 
        /// Note: The request appears to hang in Swagger, but it's actually timing out
        /// Check the browser console or network tab to see the 408 response after 30 seconds
        /// </summary>
        [HttpGet("slow")]
        public async Task<IActionResult> SlowTest()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(40), HttpContext.RequestAborted);
                return Ok(new { message = "Slow operation completed (this won't be reached)" });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(408, new
                {
                    error = new
                    {
                        type = "RequestTimeout",
                        message = "Request was canceled due to timeout",
                        friendlyMessage = "Your request took too long to process.",
                        configuredTimeoutSeconds = 30
                    }
                });
            }
        }

        /// <summary>
        /// Configurable delay endpoint for testing different timeout scenarios
        /// Uses DEFAULT policy (automatic) - this is a standard endpoint for testing
        /// 
        /// HOW TO TEST ON SWAGGER:
        /// 1. Open Swagger → RateLimitingTest → delay/{seconds}
        /// 2. Enter a number (e.g., 5, 35, 40)
        /// 3. Click "Execute"
        /// 4. Wait for response
        /// 
        /// Examples:
        /// - /api/ratelimitingtest/delay/5 → Returns 200 OK after ~5 seconds
        /// - /api/ratelimitingtest/delay/30 → Returns 200 OK (just within timeout)
        /// - /api/ratelimitingtest/delay/35 → Returns 408 after ~30 seconds
        /// - /api/ratelimitingtest/delay/40 → Returns 408 after ~30 seconds
        /// </summary>
        [HttpGet("delay/{seconds}")]
        public async Task<IActionResult> DelayTest(int seconds)
        {
            if (seconds < 0 || seconds > 120)
            {
                return BadRequest(new { error = "Seconds must be between 0 and 120" });
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(seconds), HttpContext.RequestAborted);
                stopwatch.Stop();
                return Ok(new
                {
                    message = "Delay completed successfully",
                    requestedSeconds = seconds,
                    actualMilliseconds = stopwatch.ElapsedMilliseconds,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (OperationCanceledException)
            {
                stopwatch.Stop();
                return StatusCode(408, new
                {
                    error = new
                    {
                        type = "RequestTimeout",
                        message = "Request was canceled",
                        requestedSeconds = seconds,
                        actualMilliseconds = stopwatch.ElapsedMilliseconds,
                        timestamp = DateTime.UtcNow
                    }
                });
            }
        }
    }
}
