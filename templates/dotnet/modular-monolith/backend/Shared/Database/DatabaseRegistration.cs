using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

    public static async Task ApplyDevelopmentMigrations<TContext>(this WebApplication app)
        where TContext : DbContext
    {
        if (app.Environment.IsDevelopment())
        {
            await using var scope = app.Services.CreateAsyncScope();
            await using var db = scope.ServiceProvider.GetRequiredService<TContext>();
            await db.Database.MigrateAsync();
        }
    }
}
