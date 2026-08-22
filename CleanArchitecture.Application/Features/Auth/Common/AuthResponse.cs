namespace CleanArchitecture.Application.Features.Auth.Common;

public sealed record AuthResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    CurrentUserDto User);
