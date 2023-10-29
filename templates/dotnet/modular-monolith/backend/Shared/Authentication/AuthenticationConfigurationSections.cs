namespace Shared.Authentication;

public static class AuthenticationConfigurationSections
{
    public const string JwtKey = "JWT_KEY";
    public const string JwtIssuer = "JWT_ISSUER";
    public const string JwtAudience = "JWT_AUDIENCE";
    public const string JwtLifetimeInSeconds = "JWT_LIFETIME_SECONDS";
}
