using Accounts.Database;
using Accounts.Database.Entities;
using Accounts.Services;
using Shared.Configuration;
using Shared.Database;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts;

public static class ModuleRegistration
{
    public static IServiceCollection AddAccountsModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDatabase<AccountsDbContext>(configuration, AccountsDbContext.Schema)
            .AddValidatorsFromAssemblyContaining<AccountsDbContext>()
            .AddTransient<JwtAuthService>()
            .AddTransient<AccountEmailService>();

        var passwordOptions = configuration.GetValueOrThrow<PasswordOptions>(AccountsConfigurationSections.Password);
        var signInOptions = configuration.GetValueOrThrow<SignInOptions>(AccountsConfigurationSections.SignIn);
        var userOptions = configuration.GetValueOrThrow<UserOptions>(AccountsConfigurationSections.User);

        services
            .AddSingleton(passwordOptions)
            .AddSingleton(signInOptions)
            .AddSingleton(userOptions);

        services.AddIdentityCore<User>(options =>
        {
            options.Password = passwordOptions;
            options.SignIn = signInOptions;
            options.User = userOptions;
        })
        .AddRoles<Role>()
        .AddEntityFrameworkStores<AccountsDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
