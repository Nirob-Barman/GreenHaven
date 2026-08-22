namespace CleanArchitecture.Application.Features.Auth.Common;

public sealed record LoginRequest(
    string Email,
    string Password);
