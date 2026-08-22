namespace CleanArchitecture.Application.Features.Auth.Common;

public sealed record UpdateProfileRequest(
    string FullName,
    string? PhoneNumber,
    string? Address);
