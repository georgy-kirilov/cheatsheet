using Accounts.Database.Entities;
using Accounts.Services;
using Shared.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace Accounts.Features;

public static class SendEmailConfirmation
{
    public sealed class Endpoint : IEndpoint
    {
        public void Map(IEndpointRouteBuilder builder) =>
            builder
            .MapGet("api/accounts/send-email-confirmation", Handle)
            .AllowAnonymous()
            .WithTags("Accounts");
    }

    public static async Task<IResult> Handle(
        string email,
        UserManager<User> userManager,
        AccountEmailService accountEmailService)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return Results.NotFound();
        }

        await accountEmailService.SendEmailConfirmation(user);

        return Results.Ok();
    }
}
