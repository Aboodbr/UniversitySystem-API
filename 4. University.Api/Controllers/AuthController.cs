using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.DTOs.Auth;

namespace University.Api.Controllers;

public class AuthController : BaseApiController
{
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginRequestDto request)
    {
        // Placeholder for real logic
        return Ok(new ApiResponse<AuthResponseDto>(new AuthResponseDto { Token = "mock_token" }, "Login successful"));
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register(RegisterRequestDto request)
    {
        // Placeholder for real logic
        return Ok(new ApiResponse<AuthResponseDto>(new AuthResponseDto { Token = "mock_token" }, "Registration successful"));
    }
}
