namespace CleanArchitecture.Application.Features.Users.Common;

public sealed record AdminUserDto(
    string Id,
    string Email,
    string? FullName,
    string? PhoneNumber,
    string? Address,
    IReadOnlyCollection<string> Roles);
