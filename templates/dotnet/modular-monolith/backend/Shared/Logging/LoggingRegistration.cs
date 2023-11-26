using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shared.Configuration;

namespace Shared.Logging;

public static class LoggingRegistration
{
    public static void UseLogging(this IHostBuilder hostBuilder,
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
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .MinimumLevel.Warning()
                .WriteTo.Seq(seqHost)
                .CreateLogger();
        }

        hostBuilder.UseSerilog();
    }
}
