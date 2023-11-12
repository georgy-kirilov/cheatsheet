using Accounts;
using Shared.Api;
using Shared.Authentication;
using Shared.Configuration;
using Shared.DataProtection;
using Shared.Logging;
using Shared.Messaging;
using Shared.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppLogging(builder.Environment);

builder.Configuration.Sources.Clear();
builder.Configuration
    .AddAppSettingsFor(nameof(Accounts))
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .AddAppSwagger<Program>()
    .AddAppAuthentication(builder.Configuration)
    .AddAppDataProtection(builder.Configuration)
    .AddAppLogging()
    .AddAppMessaging(builder.Configuration, nameof(Accounts))
    .AddAppValidation(nameof(Accounts));

builder.Services.AddAccountsModule(builder.Configuration);

var app = builder.Build();

app.UseAppSwagger();
app.UseAuthorization();
app.MapAppEndpoints(nameof(Accounts));

app.Run();
