namespace CleanArchitecture.Application.Features.Auth.Common;

public sealed record RegisterRequest(
    string FullName,
    string Email,
    string Password,
    string? PhoneNumber,
    string? Address);
