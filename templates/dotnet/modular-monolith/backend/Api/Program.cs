using Shared.Api;
using Shared.Authentication;
using Shared.Configuration;
using Shared.Database;
using Shared.DataProtection;
using Shared.Email;
using Shared.Logging;
using Shared.Messaging;
using Accounts;
using Accounts.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseLogging(builder.Environment, builder.Configuration);

builder.Configuration.Sources.Clear();
builder.Configuration
    .AddAppSettings(
    [
        typeof(AccountsDbContext).Assembly
    ])
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .AddLogging()
    .AddSwagger()
    .AddAuthentication(builder.Configuration)
    .AddDataProtection(builder.Configuration)
    .AddEmail(builder.Configuration);

builder.Services.AddMessaging(builder.Configuration,
[
    typeof(AccountsDbContext).Assembly
]);

builder.Services
    .AddAccountsModule(builder.Configuration);

var application = builder.Build();

await application.ApplyMigrationsInDevelopment<AccountsDbContext>();

application
    .UseSwaggerInDevelopment()
    .UseJwtFromInsideCookie()
    .UseAuthentication()
    .UseAuthorization();

application
    .MapApiEndpoints<AccountsDbContext>();

await application.RunAsync();
