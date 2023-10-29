using Microsoft.Extensions.Configuration;

namespace Shared.Configuration;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddAppsettingFilesFor(this IConfigurationBuilder configuration, string projectName) =>
        configuration
            .AddJsonFile($"/src/{projectName}/appsettings.json", optional: false)
            .AddJsonFile($"/src/{projectName}/appsettings.Development.json", optional: false);

    public static T GetValueOrThrow<T>(this IConfiguration configuration, string section)
    {
        var sectionData = configuration.GetSection(section);

        if (sectionData is null || !sectionData.Exists())
        {
            throw new FailedToLoadConfigurationValueException(section);
        }

        return sectionData.Get<T>() ?? throw new FailedToLoadConfigurationValueException(section);
    }
}
