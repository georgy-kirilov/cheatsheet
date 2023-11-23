using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shared.Configuration;

namespace Shared.Logging;

public static class LoggingRegistration
{
    public static IServiceCollection AddAppLogging(this IServiceCollection services) => services.AddLogging();

    public static void UseAppLogging(this IHostBuilder hostBuilder,
        IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        var seqHost = configuration.GetValueOrThrow<string>(LoggingConfigurationSections.SeqHost);

        if (environment.IsDevelopment())
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(seqHost)
                .CreateLogger();
        }
        else
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(seqHost)
                .CreateLogger();
        }

        hostBuilder.UseSerilog();
    }
}
