using AzureADB2C.API.Models;

namespace AzureADB2C.API.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResult> AuthenticateAsync(string username, string password);
    Task<bool> ValidateTokenAsync(string token);
}
