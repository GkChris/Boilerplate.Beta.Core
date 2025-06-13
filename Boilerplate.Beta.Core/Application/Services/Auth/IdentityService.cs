using Boilerplate.Beta.Core.Application.Services.Abstractions.Auth;
using Boilerplate.Beta.Core.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Boilerplate.Beta.Core.Application.Services.Auth
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly FusionAuthSettings _settings;

        public IdentityService(HttpClient httpClient, IOptions<FusionAuthSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", _settings.ClientId),
                new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            var response = await _httpClient.PostAsync(_settings.TokenUrl, content);
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(result);
            return json.RootElement.GetProperty("access_token").GetString();
        }
    }
}
