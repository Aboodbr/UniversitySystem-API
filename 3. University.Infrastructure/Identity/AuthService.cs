using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using University.Application.DTOs;
using University.Application.DTOs.Auth;
using University.Application.Interfaces;
using University.Infrastructure.Identity; 

namespace University.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    // Inject UserManager provided by ASP.NET Core Identity
    public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request)
    {
        // 1. Find user by email
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return new ApiResponse<AuthResponseDto>("Invalid email or password.");

        // 2. Verify password using Identity's built-in hasher
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            return new ApiResponse<AuthResponseDto>("Invalid email or password.");

        // 3. Get user roles (Assuming the user has at least one role)
        var userRoles = await _userManager.GetRolesAsync(user);
        var role = userRoles.Count > 0 ? userRoles[0] : "Student";

        // 4. Generate token
        string token = GenerateJwtToken(user.Id, user.Email, role);
        var response = new AuthResponseDto 
        { 
            Token = token,
            Email = user.Email,
            UserId = user.Id
        };

        return new ApiResponse<AuthResponseDto>(response, "Login successful.");
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(
     RegisterRequestDto request)
    {
        // 1. Check if email already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            return new ApiResponse<AuthResponseDto>(
                "Email is already registered."
            );

        // 2. Create user
        var newUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        // 3. Create user with hashed password
        var result = await _userManager.CreateAsync(
            newUser,
            request.Password
        );

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(e => e.Description)
                .ToList();

            var response = new ApiResponse<AuthResponseDto>(
                "User registration failed."
            );

            response.Errors.AddRange(errors);

            return response;
        }

        // 4. Assign role
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            var roleExists = await _userManager
                .GetRolesAsync(newUser);

            var roleResult = await _userManager.AddToRoleAsync(
                newUser,
                request.Role
            );

            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors
                    .Select(e => e.Description)
                    .ToList();

                // Remove user if role assignment fails
                await _userManager.DeleteAsync(newUser);

                var response = new ApiResponse<AuthResponseDto>(
                    "Failed to assign user role."
                );

                response.Errors.AddRange(errors);

                return response;
            }
        }

        // 5. Generate JWT
        var token = GenerateJwtToken(
            newUser.Id,
            newUser.Email!,
            request.Role ?? "Student"
        );

        var authResponse = new AuthResponseDto
        {
            Token = token,
            Email = newUser.Email,
            UserId = newUser.Id
        };

        return new ApiResponse<AuthResponseDto>(
            authResponse,
            "Registration successful."
        );
    }

    /// <summary>
    /// Generates a securely signed JWT token containing user claims (Payload).
    /// </summary>
    private string GenerateJwtToken(string userId, string email, string role)
    {
        var secretKey = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24), // Token valid for 24 hours
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}