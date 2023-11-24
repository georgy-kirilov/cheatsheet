using Accounts.Database.Entities;
using FluentValidation;
using FluentValidation.Results;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Shared.Api;

namespace Accounts.Features;

public static class Register
{
    public sealed class Endpoint : IEndpoint
    {
        public void Map(IEndpointRouteBuilder builder) =>
            builder
            .MapPost("api/accounts/register", Handle)
            .AllowAnonymous()
            .WithTags("Accounts");
    }

    public static async Task<Results<Ok, BadRequest<ValidationFailure[]>>> Handle(
        Request request,
        Validator validator,
        UserManager<User> userManager,
        IPublishEndpoint bus,
        CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(validationResult.Errors.ToArray());
        }

        User user = new()
        {
            UserName = request.Username,
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

            return TypedResults.BadRequest(identityErrors.ToArray());
        }

        await bus.Publish(new UserAccountCreatedMessage(user), cancellationToken);

        return TypedResults.Ok();
    }

    public sealed record Request
    (
        string Username,
        string Email,
        string Password,
        string ConfirmPassword
    );

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Username)
                .NotEmpty();
                
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Equal(x => x.ConfirmPassword);
        }
    }
}
