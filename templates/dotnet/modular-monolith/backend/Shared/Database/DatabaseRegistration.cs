using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configuration;

namespace Shared.Database;

public static class DatabaseRegistration
{
    public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services,
        IConfiguration configuration,
        string schema,
        string section = DatabaseConfigurationSections.DefaultConnection)
        where TContext : BaseDbContext
    {
        var connectionString = configuration.GetValueOrThrow<string>(section);

        services.AddDbContext<TContext>(dbContextOptions =>
        {
            dbContextOptions.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema)
                    .MigrationsAssembly(typeof(TContext).Assembly.FullName);
            })
            .UseSnakeCaseNamingConvention();
        }); 
        
        return services;
    }
}
