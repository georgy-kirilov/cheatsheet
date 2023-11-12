using Accounts.Database.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Shared.Api;
using Shared.Authentication;

namespace Accounts.Features;

public static class Login
{
    public sealed class Endpoint : IEndpoint
    {
        public void Map(IEndpointRouteBuilder builder) => builder
            .MapPost("/api/accounts/login", Handle)
            .AllowAnonymous()
            .WithTags("Accounts");
    }

    public static async Task<Results<Ok<Response>, BadRequest<ValidationFailure>>> Handle(
        HttpContext context,
        Request request,
        UserManager<User> userManager,
        JwtAuthService jwtAuthService)
    {
        User? user = await userManager.FindByNameAsync(request.Username);

        if (user is null)
        {
            return TypedResults.BadRequest(Errors.InvalidLoginCredentials);
        }

        string token = jwtAuthService.GenerateJwtToken(user.Id, user.UserName!);

        jwtAuthService.AppendJwtAuthCookie(context, token);

        return TypedResults.Ok(new Response(token));
    }

    public static class Errors
    {
        public static readonly ValidationFailure InvalidLoginCredentials = new(string.Empty, "Invalid username or password.")
        {
            ErrorCode = "InvalidLoginCredentials"
        };
    }

    public sealed record Request(string Username, string Password);

    public sealed record Response(string Token);
}
