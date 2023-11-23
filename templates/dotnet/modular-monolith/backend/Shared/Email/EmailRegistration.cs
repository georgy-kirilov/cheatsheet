using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configuration;

namespace Shared.Email;

public static class EmailRegistration
{
    public static IServiceCollection AddAppEmail(this IServiceCollection services, IConfiguration configuration)
    {
        var sendGridApiKey = configuration.GetValueOrThrow<string>(EmailConfigurationSections.SendGridApiKey);

        var sendGridSettings = new SendGridSettings { ApiKey = sendGridApiKey };

        services.AddSingleton(sendGridSettings);

        services.AddTransient<IEmailSender, SendGridEmailSender>();

        return services;
    }
}
