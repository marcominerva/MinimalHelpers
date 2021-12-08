using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace MinimalRegistration;

public static class WebApplicationExtensions
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        RegisterEndpoints(app, Assembly.GetCallingAssembly());
    }

    public static void RegisterEndpointsFromAssemblyContaining<T>(this WebApplication app) where T : class
    {
        RegisterEndpoints(app, typeof(T).Assembly);
    }

    public static void RegisterEndpoints(this WebApplication app, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(assembly);

        var routeEndpointHandlerInterfaceType = typeof(IRouteEndpointHandler);

        var routeEndpointHandlerTypes = assembly.GetTypes().Where(t =>
            t.IsClass && !t.IsAbstract
            && t.GetConstructor(Type.EmptyTypes) != null
            && routeEndpointHandlerInterfaceType.IsAssignableFrom(t));

        foreach (var routeEndpointHandlerType in routeEndpointHandlerTypes)
        {
            var instantiatedType = (IRouteEndpointHandler)Activator.CreateInstance(routeEndpointHandlerType)!;
            instantiatedType.Map(app);
        }
    }
}
