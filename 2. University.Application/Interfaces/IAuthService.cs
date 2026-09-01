using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.DTOs.Auth;

namespace University.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request);
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request);
}