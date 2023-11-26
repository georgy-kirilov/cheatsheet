using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Configuration;

namespace Shared.Authentication;

public static class AuthenticationRegistration
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
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

        services.AddAuthorization();

        return services;
    }

    public static WebApplication UseJwtFromInsideCookie(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            if (!context.Request.Headers.ContainsKey(JwtAuthConstants.Header))
            {
                var cookie = context.Request.Cookies[JwtAuthConstants.Cookie];

                if (cookie is not null)
                {
                    context.Request.Headers.Append(JwtAuthConstants.Header, $"Bearer {cookie}");
                }
            }

            await next(context);
        });

        return app;
    }
}
