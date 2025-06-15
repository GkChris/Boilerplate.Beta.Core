using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers.TestControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProtectedTestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IIdentityService _identityClient;

        public ProtectedTestController(IHttpClientFactory httpClientFactory, IIdentityService identityClient)
        {
            _httpClientFactory = httpClientFactory;
            _identityClient = identityClient;
        }

        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok(new { message = "Anyone can access this endpoint." });
        }

        [Authorize]
        [HttpGet("get-userinfo")]
        public async Task<IActionResult> GetUserinfo()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var userInfoJson = await _identityClient.FetchUserInfoAsync(token);
            if (userInfoJson == null)
            {
                return Unauthorized("Failed to fetch user info.");
            }

            return Ok(userInfoJson.RootElement.ToString());
        }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            return Ok($"Hello {User.Identity?.Name ?? "Unknown user"}");
        }

        [Authorize(Policy = "ActiveUser")]
        [HttpGet("protected-require-post-validation")]
        public IActionResult ProtectedRequirePostValidation()
        {
            return Ok($"Hello {User.Identity?.Name ?? "Unknown user"}");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin")]
        public IActionResult AdminOnly() => Ok("You have admin access.");

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _identityClient.LoginAsync(request.Username, request.Password);
            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { access_token = token });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
