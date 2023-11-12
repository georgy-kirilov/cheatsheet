namespace Accounts.Features;

public sealed record UserRegisteredMessage(string Username, Guid UserId);
