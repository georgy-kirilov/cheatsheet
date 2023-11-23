using Microsoft.Extensions.Logging;
using MassTransit;

namespace Accounts.Features;

public sealed record UserRegisteredMessage(string Username, Guid UserId);

public sealed class UserRegisteredMessageConsumer(
    ILogger<UserRegisteredMessageConsumer> logger) : IConsumer<UserRegisteredMessage>
{
    public Task Consume(ConsumeContext<UserRegisteredMessage> context)
    {
        logger.LogInformation("User '{Username}' was registered with ID '{UserId}'.",
            context.Message.Username,
            context.Message.UserId);

        return Task.CompletedTask;
    }
}
