using MassTransit;
using Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging;

public static class MessagingRegistration
{
    public static IServiceCollection AddMessaging(this IServiceCollection services,
        IConfiguration configuration,
        Assembly[] consumerAssemblies)
    {
        var host = configuration.GetValueOrThrow<string>(MessagingConfigurationSections.RabbitMqHost);
        var username = configuration.GetValueOrThrow<string>(MessagingConfigurationSections.RabbitMqUsername);
        var password = configuration.GetValueOrThrow<string>(MessagingConfigurationSections.RabbitMqPassword);

        services.AddMassTransit(bus =>
        {
            var consumerTypes = consumerAssemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IConsumer).IsAssignableFrom(t) &&
                    !t.IsInterface &&
                    !t.IsAbstract);

            foreach (var consumerType in consumerTypes)
            {
                bus.AddConsumer(consumerType);
            }

            bus.SetKebabCaseEndpointNameFormatter();

            bus.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host, "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
