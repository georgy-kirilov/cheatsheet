using Accounts;
using Shared.Api;
using Shared.Authentication;
using Shared.Configuration;
using Shared.DataProtection;
using Shared.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration
    .AddAppSettingsFor(nameof(Accounts))
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .AddAppSwagger<Program>()
    .AddAppDataProtection(builder.Configuration)
    .AddAppAuthentication(builder.Configuration)
    .AddAppValidation();

builder.Services.AddAccountsModule(builder.Configuration);

var app = builder.Build();

app.UseAppSwagger();
app.UseAuthorization();
app.MapAppEndpoints();

app.Run();
