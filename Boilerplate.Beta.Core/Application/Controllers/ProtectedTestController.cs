using Boilerplate.Beta.Core.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Boilerplate.Beta.Core.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProtectedTestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FusionAuthSettings _fusionAuthOptions;

        public ProtectedTestController(IHttpClientFactory httpClientFactory, IOptions<FusionAuthSettings> fusionAuthOptions)
        {
            _httpClientFactory = httpClientFactory;
            _fusionAuthOptions = fusionAuthOptions.Value;
        }

        [HttpGet("public")]
        public IActionResult Public() => Ok("Anyone can access this");


        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            var token = Request.Headers["Authorization"].ToString();
            return Ok($"Hello {User.Identity?.Name}, Token received: {token}");
        }


        [Authorize(Roles = "admin")]
        [HttpGet("admin")]
        public IActionResult AdminOnly() => Ok("You have admin access.");

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient("fusionauth");

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", _fusionAuthOptions.ClientId),
                new KeyValuePair<string, string>("client_secret", _fusionAuthOptions.ClientSecret),
                new KeyValuePair<string, string>("username", request.Username),
                new KeyValuePair<string, string>("password", request.Password),
            });

            var response = await client.PostAsync(_fusionAuthOptions.TokenUrl, content);

            if (!response.IsSuccessStatusCode)
                return Unauthorized(await response.Content.ReadAsStringAsync());

            var result = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(result);
            var token = json.RootElement.GetProperty("access_token").GetString();

            return Ok(new { access_token = token });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
