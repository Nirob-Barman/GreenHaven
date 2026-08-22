namespace CleanArchitecture.Infrastructure.Identity;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string Key { get; init; } = string.Empty;

    public int ExpiresMinutes { get; init; } = 60;

    public int RefreshTokenExpiresDays { get; init; } = 7;

    public string RefreshTokenCookieName { get; init; } = "refreshToken";
}
