namespace Staffinity.Personal.Infrastructure.Security.Jwt;

public sealed class JwtSettings
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Secret { get; init; } = string.Empty;
    public int ExpiresMinutes { get; init; } = 60;
}
