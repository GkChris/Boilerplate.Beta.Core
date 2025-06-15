using Boilerplate.Beta.Core.Infrastructure.Auth.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Boilerplate.Beta.Core.Infrastructure.Auth
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly AuthSettings _settings;
        private readonly HttpClient _httpClient;

        public TokenValidationService(IOptions<AuthSettings> settings, IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            _httpClient = httpClientFactory.CreateClient(_settings.ClientName);
        }

        public async Task<bool> ValidateTokenActiveAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            var introspectUrl = new Uri(new Uri(_settings.Authority), _settings.IntrospectEndpoint);

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("token", token),
                new KeyValuePair<string, string>("client_id", _settings.ClientId),
                new KeyValuePair<string, string>("client_secret", _settings.ClientSecret)
            });

            using var request = new HttpRequestMessage(HttpMethod.Post, introspectUrl)
            {
                Content = content
            };

            try
            {
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return false;

                var jsonString = await response.Content.ReadAsStringAsync();
                using var json = JsonDocument.Parse(jsonString);

                if (json.RootElement.TryGetProperty("active", out var activeProperty))
                {
                    return activeProperty.GetBoolean();
                }

                return false;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
