using Accounts;
using Shared.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();

builder.Configuration
    .AddJsonFile("/src/Accounts/appsettings.json", optional: true)
    .AddJsonFile("/src/Accounts/appsettings.Development.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .AddAppAuthentication(builder.Configuration)
    .AddAccountsModule(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
