using FixFlow.Application.Common;
using FixFlow.Application.DTOs;

namespace FixFlow.Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> RegisterAsync(
        RegisterDto dto,
        CancellationToken cancellationToken = default);

    Task<Result<AuthResponseDto>> LoginAsync(
        LoginDto dto,
        CancellationToken cancellationToken = default);
}