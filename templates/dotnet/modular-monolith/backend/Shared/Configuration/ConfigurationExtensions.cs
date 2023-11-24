using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Shared.Configuration;

public static class ConfigurationExtensions
{
    public static T GetValueOrThrow<T>(this IConfiguration configuration, string section)
    {
        var sectionData = configuration.GetSection(section);

        if (sectionData is null || !sectionData.Exists())
        {
            throw new FailedToLoadConfigurationValueException(section);
        }

        return sectionData.Get<T>() ?? throw new FailedToLoadConfigurationValueException(section);
    }

    public static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder configuration,
        Assembly[] appsettingsAssemblies)
    {
        foreach (var projectName in appsettingsAssemblies.Select(a => a.GetName().Name))
        {
            configuration.AddJsonFile($"/src/{projectName}/appsettings.json", optional: false);
            configuration.AddJsonFile($"/src/{projectName}/appsettings.Development.json", optional: false);
        }

        return configuration;
    }
}
