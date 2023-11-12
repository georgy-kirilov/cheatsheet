using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Validation;

public static class ValidationRegistration
{
    public static IServiceCollection AddAppValidation<TProgram>(this IServiceCollection services) =>
        services.AddAppValidation(typeof(TProgram).Assembly.FullName!);
    
    public static IServiceCollection AddAppValidation(this IServiceCollection services,
        params string[] validatorsAssemblyNames)
    {
        var assemblies = validatorsAssemblyNames.Distinct().Select(Assembly.Load);
        services.AddValidatorsFromAssemblies(assemblies);
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        return services;
    }
}
