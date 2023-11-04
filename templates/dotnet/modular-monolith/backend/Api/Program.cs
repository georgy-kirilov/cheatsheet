using Accounts;
using Shared.Api;
using Shared.Authentication;
using Shared.Configuration;
using Shared.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration
    .AddAppSettingsFor(nameof(Accounts))
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .AddAppSwagger<Program>()
    .AddAppDataProtection(builder.Configuration)
    .AddAppAuthentication(builder.Configuration);

builder.Services.AddAccountsModule(builder.Configuration);

var app = builder.Build();

app.UseAppSwagger();
app.UseAuthorization();

app.Run();
