using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace MinimalHelpers.Registration;

public static class WebApplicationExtensions
{
    public static void MapEndpoints(this WebApplication app)
        => MapEndpoints(app, Assembly.GetCallingAssembly());

    public static void MapEndpointsFromAssemblyContaining<T>(this WebApplication app) where T : class
        => MapEndpoints(app, typeof(T).Assembly);

    public static void MapEndpoints(this WebApplication app, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(assembly);

        var routeEndpointHandlerInterfaceType = typeof(IRouteEndpointHandler);

        var routeEndpointHandlerTypes = assembly.GetTypes().Where(t =>
            t.IsClass && !t.IsAbstract && !t.IsGenericType
            && t.GetConstructor(Type.EmptyTypes) != null
            && routeEndpointHandlerInterfaceType.IsAssignableFrom(t));

        foreach (var routeEndpointHandlerType in routeEndpointHandlerTypes)
        {
            var instantiatedType = (IRouteEndpointHandler)Activator.CreateInstance(routeEndpointHandlerType)!;
            instantiatedType.Map(app);
        }
    }
}