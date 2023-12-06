using Accounts.Database.Entities;
using Shared.Api;
using MassTransit;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace Accounts.Features;

public static class Register
{
    public sealed class Endpoint : IEndpoint
    {
        public void Map(IEndpointRouteBuilder builder) =>
            builder
            .MapPost("accounts/register", Handle)
            .AllowAnonymous()
            .WithTags("Accounts");
    }

    public static async Task<IResult> Handle(
        Request request,
        Validator validator,
        UserManager<User> userManager,
        IBus bus,
        CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email
        };

        var identityResult = await userManager.CreateAsync(user, request.Password);

        if (!identityResult.Succeeded)
        {
            var identityErrors = identityResult.Errors.Select(err => new ValidationFailure
            {
                ErrorCode = err.Code,
                ErrorMessage = err.Description
            });

            return Results.BadRequest(identityErrors);
        }

        await bus.Publish(new UserAccountCreatedMessage(user), cancellationToken);

        return Results.Ok();
    }

    public sealed record Request
    (
        string Email,
        string Password,
        string ConfirmPassword
    );

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {       
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Equal(x => x.ConfirmPassword);
        }
    }
}
