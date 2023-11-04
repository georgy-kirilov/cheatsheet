using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        services.AddScoped<JwtGenerator>();

        services
            .AddSingleton(jwtSettings)
            .AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            })
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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

        return services;
    }
}
