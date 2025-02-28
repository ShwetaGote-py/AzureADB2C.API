using System.ComponentModel.DataAnnotations;

namespace AzureADB2C.API.Models;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
