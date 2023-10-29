using Accounts;
using Shared.Authentication;
using Shared.Configuration;
using Shared.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();

builder.Configuration
    .AddAppsettingFilesFor("Accounts")
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .AddAppDataProtection(builder.Configuration)
    .AddAppAuthentication(builder.Configuration);

builder.Services
    .AddAccountsModule(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
