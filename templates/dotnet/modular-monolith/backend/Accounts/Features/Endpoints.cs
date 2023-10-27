using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Accounts.Features;

public static class Endpoints
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/accounts").RequireAuthorization();

        group.MapPost("/register", Register.Handle).AllowAnonymous();
    }
}
