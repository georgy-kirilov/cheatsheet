using Accounts.Database.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Shared.Api;
using Shared.Authentication;

namespace Accounts.Features;

public static class GetMineUserInfo
{
    public sealed class Endpoint : IEndpoint
    {
        public void Map(IEndpointRouteBuilder builder) => builder
            .MapGet("/api/accounts/me/info", Handle)
            .RequireAuthorization()
            .WithTags("Accounts");
    }

    public static async Task<Ok<Response>> Handle(
        HttpContext http,
        UserManager<User> userManager)
    {
        var userId = http.User.GetUserId().ToString();

        User user = await userManager.FindByIdAsync(userId) ?? throw new InvalidOperationException();

        return TypedResults.Ok(new Response
        (
            user.Email,
            user.UserName
        ));
    }

    public sealed record Response(string? Email, string? Username);
}
