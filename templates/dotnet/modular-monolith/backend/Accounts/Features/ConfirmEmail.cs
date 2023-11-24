using Accounts.Database.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Shared.Api;

namespace Accounts.Features;

public static class ConfirmEmail
{
    public sealed class Endpoint : IEndpoint
    {
        public const string Route = "api/accounts/confirm-email";

        public void Map(IEndpointRouteBuilder builder) =>
            builder
            .MapGet(Route, Handle)
            .AllowAnonymous()
            .WithTags("Accounts");
    }

    public static async Task<IResult> Handle(Guid userId, string token, UserManager<User> userManager)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        var identityResult = await userManager.ConfirmEmailAsync(user, token);

        if (!identityResult.Succeeded)
        {
            return TypedResults.BadRequest(identityResult.Errors);
        }

        return TypedResults.Ok();
    }
}
