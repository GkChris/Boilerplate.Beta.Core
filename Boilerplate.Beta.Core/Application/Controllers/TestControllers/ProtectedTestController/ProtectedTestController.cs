using Boilerplate.Beta.Core.Application.Controllers.TestControllers.ProtectedTest.DTOs;
using Boilerplate.Beta.Core.Application.Controllers.TestControllers.ProtectedTest.Services;
using Boilerplate.Beta.Core.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers.TestControllers.ProtectedTestController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProtectedTestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IIdentityService _identityService;

        public ProtectedTestController(IHttpClientFactory httpClientFactory, IIdentityService identityService)
        {
            _httpClientFactory = httpClientFactory;
            _identityService = identityService;
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

            var userInfoJson = await _identityService.FetchUserInfoAsync(token);
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
            var token = await _identityService.LoginAsync(request.Username, request.Password);
            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { access_token = token });
        }


        [HttpPost("social-login")]
        public async Task<IActionResult> SocialLogin([FromBody] SocialLoginRequest request)
        {
            var token = await _identityService.LoginWithSocialAsync(request.Code, request.RedirectUri);
            if (token == null) return Unauthorized("Invalid or expired auth code");

            return Ok(new { access_token = token });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var success = await _identityService.LogoutAsync();
            if (!success) return StatusCode(500, "Failed to logout");

            return NoContent();
        }
    }
}
