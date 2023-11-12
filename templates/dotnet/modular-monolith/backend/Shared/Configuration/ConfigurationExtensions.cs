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

    public static IConfigurationBuilder AddAppSettingsFor<TProgram>(this IConfigurationBuilder configuration) =>
        configuration.AddAppSettingsFor(typeof(TProgram).Assembly.FullName!);

    public static IConfigurationBuilder AddAppSettingsFor(this IConfigurationBuilder configuration,
        params string[] projectNames) 
    {
        foreach (var projectName in projectNames.Distinct())
        {
            configuration.AddJsonFile($"/src/{projectName}/appsettings.json", optional: false);
            configuration.AddJsonFile($"/src/{projectName}/appsettings.Development.json", optional: false);
        }

        return configuration;
    }
}
