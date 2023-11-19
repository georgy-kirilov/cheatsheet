using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Accounts.Database;
using Accounts.Database.Entities;
using Accounts.Services;
using Shared.Configuration;
using Shared.Database;

namespace Accounts;

public static class ModuleRegistration
{
    public static IServiceCollection AddAccountsModule(this IServiceCollection services, IConfiguration configuration)
    {   
        var connectionString = configuration.GetValueOrThrow<string>(DatabaseConfigurationSections.DefaultConnection);

        services.AddDbContext<AccountsDbContext>(dbOptions =>
        {
            dbOptions.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, AccountsDbContext.Schema)
                    .MigrationsAssembly(typeof(AccountsDbContext).Assembly.FullName);
            })
            .UseSnakeCaseNamingConvention();
        });

        services.AddIdentityCore<User>(options =>
        {
            options.Password = configuration.GetValueOrThrow<PasswordOptions>(AccountsConfigurationSections.Password);
            options.SignIn = configuration.GetValueOrThrow<SignInOptions>(AccountsConfigurationSections.SignIn);
            options.User = configuration.GetValueOrThrow<UserOptions>(AccountsConfigurationSections.User);
        })
        .AddRoles<Role>()
        .AddEntityFrameworkStores<AccountsDbContext>();

        services.AddTransient<JwtAuthService>();

        return services;
    }
}
