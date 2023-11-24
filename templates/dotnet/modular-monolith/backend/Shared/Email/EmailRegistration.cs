using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configuration;

namespace Shared.Email;

public static class EmailRegistration
{
    public static IServiceCollection AddAppEmail(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new EmailSettings
        {
            ApiKey = configuration.GetValueOrThrow<string>(EmailConfigurationSections.EmailApiKey),
            FromEmail = configuration.GetValueOrThrow<string>(EmailConfigurationSections.EmailFromAddress),
            FromName = configuration.GetValueOrThrow<string>(EmailConfigurationSections.EmailFromName)
        });

        services.AddTransient<IEmailSender, SendGridEmailSender>();

        return services;
    }
}
