using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.DTOs.Auth;
using University.Application.Interfaces;

namespace University.Api.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    // Constructor for Dependency Injection
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticates a user and generates a JWT token upon successful login.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);

        // Return 400 Bad Request if authentication fails (e.g., invalid email or password)
        if (!response.Success)
            return BadRequest(response);

        // Return 200 OK with the generated token
        return Ok(response);
    }

    /// <summary>
    /// Registers a new user (Student or Professor) and generates an initial JWT token.
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto request)
    {
        var response = await _authService.RegisterAsync(request);

        // Return 400 Bad Request if registration fails (e.g., email already exists in the system)
        if (!response.Success)
            return BadRequest(response);

        // Return 200 OK with the generated token for immediate login after registration
        return Ok(response);
    }
}