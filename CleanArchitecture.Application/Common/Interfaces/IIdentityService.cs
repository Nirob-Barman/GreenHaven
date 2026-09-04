using CleanArchitecture.Application.Features.Auth.Common;
using CleanArchitecture.Application.Features.Users.Common;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<AuthResult> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken);

    Task<AuthResult> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken);

    Task<AuthResult> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken);

    Task LogoutAsync(
        string refreshToken,
        CancellationToken cancellationToken);

    Task<CurrentUserDto> GetCurrentUserAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<CurrentUserDto> UpdateCurrentUserAsync(
        string userId,
        UpdateProfileRequest request,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<AdminUserDto>> GetUsersAsync(
        CancellationToken cancellationToken);

    Task<AdminUserDto> GetUserByIdAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<AdminUserDto> UpdateUserRolesAsync(
        string userId,
        IReadOnlyCollection<string> roles,
        CancellationToken cancellationToken);
}
