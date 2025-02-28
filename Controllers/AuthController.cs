using AzureADB2C.API.Models;
using AzureADB2C.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzureADB2C.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthenticationService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResult>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.AuthenticateAsync(request.Username, request.Password);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for user {Username}", request.Username);
            return BadRequest(new { error = "Invalid credentials" });
        }
    }

    [Authorize]
    [HttpGet("validate")]
    public async Task<ActionResult<bool>> ValidateToken()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return BadRequest(new { error = "Invalid token" });
        }

        var token = authHeader.Substring("Bearer ".Length);
        var isValid = await _authService.ValidateTokenAsync(token);
        
        return Ok(isValid);
    }
}
