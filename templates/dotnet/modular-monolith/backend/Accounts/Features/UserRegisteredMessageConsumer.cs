using MassTransit;
using Microsoft.Extensions.Logging;

namespace Accounts.Features;

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
