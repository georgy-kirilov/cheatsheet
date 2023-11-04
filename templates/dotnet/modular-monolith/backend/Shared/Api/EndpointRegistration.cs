using Microsoft.AspNetCore.Routing;

namespace Shared.Api;

public static class EndpointRegistration
{
    public static void MapAppEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var endpointTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic)
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                typeof(IEndpoint).IsAssignableFrom(t)
                && !t.IsInterface
                && !t.IsAbstract);

        foreach (var type in endpointTypes)
        {
            var instance = Activator.CreateInstance(type) as IEndpoint ?? throw new FailedToRegisterApiEndpointException();
            instance.Map(routeBuilder);
        }
    }
}
