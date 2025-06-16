using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Boilerplate.Beta.Core.Application.Services
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
            return json.RootElement.GetProperty("access_token").GetString();
        }

        public async Task<string?> LoginWithSocialAsync(string code, string redirectUri)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("client_id", _settings.ClientId),
                new KeyValuePair<string, string>("client_secret", _settings.ClientSecret)
            });

            var response = await _httpClient.PostAsync(_settings.TokenEndpoint, content);
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(result);
            return json.RootElement.GetProperty("access_token").GetString();
        }

        public async Task<bool> LogoutAsync()
        {
            var response = await _httpClient.GetAsync(_settings.LogoutEndpoint);
            return response.IsSuccessStatusCode;
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
    }
}