using Boilerplate.Beta.Core.Application.Services.Abstractions.Auth;
using Boilerplate.Beta.Core.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Boilerplate.Beta.Core.Application.Services.Auth
{
    public class IdentityService : IIdentityService
    {
        private readonly AuthSettings _settings;
        private readonly HttpClient _httpClient;

        public IdentityService(IOptions<AuthSettings> settings, IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            _httpClient = httpClientFactory.CreateClient(_settings.ClientName);
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
                new KeyValuePair<string, string>("scope", "openid profile")
            });

            var response = await _httpClient.PostAsync(_settings.TokenEndpoint, content);
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(result);

            var accessToken = json.RootElement.GetProperty("access_token").GetString();
            return accessToken;
        }

        public async Task<JsonDocument?> FetchUserInfoAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var userInfoUrl = new Uri(new Uri(_settings.Authority), _settings.UserInfoEndpoint);

            using var request = new HttpRequestMessage(HttpMethod.Get, userInfoUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return null;

                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonDocument.Parse(jsonString);
            }
            catch (HttpRequestException)
            {
                return null;
            }
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