using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace Shared.Api;

public static class EndpointRegistration
{
    public static void MapAppEndpoints<TProgram>(this IEndpointRouteBuilder routeBuilder) =>
        routeBuilder.MapAppEndpoints(typeof(TProgram).Assembly.FullName!);

    public static void MapAppEndpoints(this IEndpointRouteBuilder routeBuilder,
        params string[] endpointsAssemblyNames)
    {
        var endpointTypes = endpointsAssemblyNames
            .Distinct()
            .Select(Assembly.Load)
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                typeof(IEndpoint).IsAssignableFrom(t)
                && !t.IsInterface
                && !t.IsAbstract);

        foreach (var type in endpointTypes)
        {
            var instance = Activator.CreateInstance(type) as IEndpoint ?? throw new FailedToRegisterApiEndpointException(type);
            instance.Map(routeBuilder);
        }
    }
}
