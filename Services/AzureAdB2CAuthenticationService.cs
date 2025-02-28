using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AzureADB2C.API.Models;
using Microsoft.Extensions.Options;

namespace AzureADB2C.API.Services;

public class AzureAdB2CAuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly string _tokenEndpoint;

    public AzureAdB2CAuthenticationService(
        IConfiguration configuration,
        HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        
        var tenant = _configuration["AzureAdB2C:Domain"];
        var policy = _configuration["AzureAdB2C:SignUpSignInPolicyId"];
        _tokenEndpoint = $"https://{tenant}/oauth2/v2.0/token?p={policy}";
    }

    public async Task<AuthenticationResult> AuthenticateAsync(string username, string password)
    {
        try
        {
            var parameters = new Dictionary<string, string>
            {
                {"client_id", _configuration["AzureAdB2C:ClientId"]!},
                {"client_secret", _configuration["AzureAdB2C:ClientSecret"]!},
                {"grant_type", "password"},
                {"scope", string.Join(" ", _configuration.GetSection("AzureAdB2C:Scopes").Get<string[]>()!)},
                {"username", username},
                {"password", password}
            };

            var content = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync(_tokenEndpoint, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Authentication failed: {error}");
            }

            var result = await JsonSerializer.DeserializeAsync<AuthenticationResult>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return result ?? throw new Exception("Failed to deserialize authentication result");
        }
        catch (Exception ex)
        {
            throw new Exception($"Authentication failed: {ex.Message}", ex);
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            // Call a protected API endpoint to validate the token
            var response = await _httpClient.GetAsync($"https://{_configuration["AzureAdB2C:Domain"]}/api/validate");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
