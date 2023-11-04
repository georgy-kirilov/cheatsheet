using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Validation;

public static class ValidationRegistration
{
    public static IServiceCollection AddAppValidation(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic)
            .ToList();

        services.AddValidatorsFromAssemblies(assemblies);

        return services;
    }
}
