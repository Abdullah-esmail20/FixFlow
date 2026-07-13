using FixFlow.Application.Common;
using FixFlow.Application.DTOs;
using FixFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FixFlow.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(dto, cancellationToken);

        if (result.IsFailure)
            return HandleFailure(result);

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(dto, cancellationToken);

        if (result.IsFailure)
            return HandleFailure(result);

        return Ok(result.Value);
    }

    private IActionResult HandleFailure(Result result)
    {
        var response = new
        {
            error = result.Error
        };

        return result.ErrorType switch
        {
            ErrorType.NotFound => NotFound(response),
            ErrorType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, response),
            ErrorType.Conflict => Conflict(response),
            _ => BadRequest(response)
        };
    }
}