using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FixFlow.Application.Common;
using FixFlow.Application.DTOs;
using FixFlow.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FixFlow.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    private static readonly string[] AllowedRoles =
    {
        "Admin",
        "Customer",
        "Technician"
    };

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponseDto>> RegisterAsync(
        RegisterDto dto,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            return Result<AuthResponseDto>.Failure("Full name is required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return Result<AuthResponseDto>.Failure("Email is required.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            return Result<AuthResponseDto>.Failure("Password is required.");

        if (string.IsNullOrWhiteSpace(dto.Role))
            return Result<AuthResponseDto>.Failure("Role is required.");

        if (!AllowedRoles.Contains(dto.Role))
            return Result<AuthResponseDto>.Failure("Invalid role.");

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);

        if (existingUser is not null)
            return Result<AuthResponseDto>.Conflict("Email is already registered.");

        if (!await _roleManager.RoleExistsAsync(dto.Role))
            await _roleManager.CreateAsync(new IdentityRole(dto.Role));

        var user = new ApplicationUser
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim(),
            UserName = dto.Email.Trim()
        };

        var createResult = await _userManager.CreateAsync(user, dto.Password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(" | ", createResult.Errors.Select(error => error.Description));
            return Result<AuthResponseDto>.Failure(errors);
        }

        await _userManager.AddToRoleAsync(user, dto.Role);

        var token = await GenerateJwtTokenAsync(user);

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            Role = dto.Role,
            Token = token
        });
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(
        LoginDto dto,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            return Result<AuthResponseDto>.Failure("Email is required.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            return Result<AuthResponseDto>.Failure("Password is required.");

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return Result<AuthResponseDto>.Failure("Invalid email or password.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!isPasswordValid)
            return Result<AuthResponseDto>.Failure("Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? string.Empty;

        var token = await GenerateJwtTokenAsync(user);

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            Role = role,
            Token = token
        });
    }

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var secretKey = jwtSettings["SecretKey"]
            ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

        var issuer = jwtSettings["Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer is not configured.");

        var audience = jwtSettings["Audience"]
            ?? throw new InvalidOperationException("JWT Audience is not configured.");

        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "120");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? string.Empty;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}