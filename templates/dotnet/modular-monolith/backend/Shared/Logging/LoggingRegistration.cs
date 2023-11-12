using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Shared.Logging;

public static class LoggingRegistration
{
    public static IServiceCollection AddAppLogging(this IServiceCollection services) =>
        services.AddLogging();
    
    public static void ConfigureAppLogging(this IHostBuilder hostBuilder, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        if (environment.IsProduction())
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        hostBuilder.UseSerilog();
    }
}
