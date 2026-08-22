namespace CleanArchitecture.Application.Features.Auth.Common;

public sealed record CurrentUserDto(
    string Id,
    string Email,
    string? FullName,
    string? PhoneNumber,
    string? Address,
    IReadOnlyCollection<string> Roles);
