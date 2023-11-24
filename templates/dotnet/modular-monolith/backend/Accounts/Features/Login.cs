using Accounts.Database.Entities;
using Accounts.Services;
using Shared.Api;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace Accounts.Features;

public static class Login
{
    public sealed class Endpoint : IEndpoint
    {
        public void Map(IEndpointRouteBuilder builder) =>
            builder
            .MapPost("api/accounts/login", Handle)
            .AllowAnonymous()
            .WithTags("Accounts")
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> Handle(
        Request request,
        HttpContext http,
        UserManager<User> userManager,
        SignInOptions signInOptions,
        JwtAuthService jwtAuthService)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Results.BadRequest(Errors.InvalidLoginCredentials);
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Results.BadRequest(Errors.InvalidLoginCredentials);
        }

        if (signInOptions.RequireConfirmedEmail && !user.EmailConfirmed)
        {
            return Results.BadRequest(Errors.ConfirmedEmailRequired);
        }

        var token = jwtAuthService.GenerateJwtToken(user.Id, user.UserName!);

        if (request.StoreJwtInCookie)
        {
            jwtAuthService.AppendJwtAuthCookie(http, token);
            return Results.Ok();
        }

        var response = new Response(token);

        return Results.Ok(response);        
    }

    public sealed record Request(string Email, string Password, bool StoreJwtInCookie);

    public sealed record Response(string Token);

    public static class Errors
    {
        public static ValidationFailure InvalidLoginCredentials => new()
        {
            ErrorCode = "InvalidLoginCredentials",
            ErrorMessage = "Invalid email or password."
        };

        public static ValidationFailure ConfirmedEmailRequired => new()
        {
            ErrorCode = "ConfirmedEmailRequired",
            ErrorMessage = "You need to confirm your email address."
        };
    }
}
