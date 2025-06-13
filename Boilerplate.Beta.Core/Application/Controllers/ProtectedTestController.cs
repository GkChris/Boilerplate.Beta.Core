using Boilerplate.Beta.Core.Application.Services.Abstractions.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers
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
        [HttpGet("protected")]
        public IActionResult Protected()
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
