using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Shared.Configuration;

namespace Shared.Messaging;

public static class MessagingRegistration
{
    public static IServiceCollection AddAppMessaging<TProgram>(this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddAppMessaging(configuration, typeof(TProgram).Assembly.FullName!);

    public static IServiceCollection AddAppMessaging(this IServiceCollection services,
        IConfiguration configuration,
        params string[] consumersAssemblyNames)
    {
        var host = configuration.GetValueOrThrow<string>(MessagingConfigurationSections.RabbitMqHost);
        var username = configuration.GetValueOrThrow<string>(MessagingConfigurationSections.RabbitMqUsername);
        var password = configuration.GetValueOrThrow<string>(MessagingConfigurationSections.RabbitMqPassword);

        services.AddMassTransit(bus =>
        {
            var assemblies = consumersAssemblyNames.Distinct().Select(Assembly.Load);

            foreach (var assembly in assemblies)
            {
                bus.AddConsumers(assembly);
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
