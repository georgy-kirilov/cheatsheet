using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using FluentValidation.Results;
using Accounts.Database.Entities;
using Accounts.Services;
using Shared.Api;

namespace Accounts.Features;

public static class Login
{
    public sealed class Endpoint : IEndpoint
    {
        public void Map(IEndpointRouteBuilder builder) => builder
            .MapPost("api/accounts/login", Handle)
            .AllowAnonymous()
            .WithTags("Accounts");
    }

    public static async Task<Results<Ok, Ok<Response>, BadRequest<ValidationFailure>>> Handle(
        Request request,
        HttpContext http,
        UserManager<User> userManager,
        JwtAuthService jwtAuthService)
    {
        User? user = await userManager.FindByNameAsync(request.Username);

        if (user is null)
        {
            return TypedResults.BadRequest(Errors.InvalidLoginCredentials);
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            return TypedResults.BadRequest(Errors.InvalidLoginCredentials);
        }

        string token = jwtAuthService.GenerateJwtToken(user.Id, user.UserName!);

        if (request.StoreJwtAuthTokenInCookies)
        {
            jwtAuthService.AppendJwtAuthCookie(http, token);
            return TypedResults.Ok();
        }

        return TypedResults.Ok(new Response(token));
    }

    public static class Errors
    {
        public static readonly ValidationFailure InvalidLoginCredentials = new()
        {
            ErrorCode = "InvalidLoginCredentials",
            ErrorMessage = "Invalid username or password."
        };
    }

    public sealed record Request
    (
        string Username,
        string Password,
        bool StoreJwtAuthTokenInCookies
    );

    public sealed record Response(string Token);
}
