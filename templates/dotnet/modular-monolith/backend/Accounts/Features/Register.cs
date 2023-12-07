using Accounts.Database.Entities;
using Shared.Api;
using MassTransit;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

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
                PropertyName = string.Empty,
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
        public Validator(IdentityOptions identityOptions)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email address is required.")
                .EmailAddress()
                .WithMessage("Email address is not in a valid format.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(identityOptions.Password.RequiredLength)
                .WithMessage($"Password must be at least {identityOptions.Password.RequiredLength} characters long.")
                .NotEqual(x => x.Email)
                .WithMessage("Password cannot be the same as the email address.");

            if (identityOptions.Password.RequireDigit)
            {
                RuleFor(x => x.Password)
                    .Must(password => password.Any(char.IsDigit))
                    .WithMessage("Password must contain at least one digit.");
            }

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");
        }
    }
}
