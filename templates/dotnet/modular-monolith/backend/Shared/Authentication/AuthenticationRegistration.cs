using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Configuration;

namespace Shared.Authentication;

public static class AuthenticationRegistration
{
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings
        {
            Key = configuration.GetValueOrThrow<string>(AuthenticationConfigurationSections.JwtKey),
            Issuer = configuration.GetValueOrThrow<string>(AuthenticationConfigurationSections.JwtIssuer),
            Audience = configuration.GetValueOrThrow<string>(AuthenticationConfigurationSections.JwtAudience),
            LifetimeInSeconds = configuration.GetValueOrThrow<int>(AuthenticationConfigurationSections.JwtLifetimeInSeconds)
        };

        services.AddSingleton(jwtSettings);

        services.AddAuthentication(x =>
        {
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });

        var authenticationRequired = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        services
            .AddAuthorizationBuilder()
            .SetDefaultPolicy(authenticationRequired);

        return services;
    }

    public static IApplicationBuilder UseAppAuthorization(this WebApplication app) => app
        .UseMiddleware<JwtInsideCookieMiddleware>()
        .UseAuthentication()
        .UseAuthorization();
}
