namespace CleanArchitecture.Application.Features.Auth.Common;

public sealed record AuthResult(
    AuthResponse Response,
    string RefreshToken);
