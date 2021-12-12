using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace MinimalHelpers.Registration;

public static class IEndpointRouteBuilderExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder app, Func<Type, bool>? predicate = null)
        => MapEndpoints(app, Assembly.GetCallingAssembly(), predicate);

    public static void MapEndpointsFromAssemblyContaining<T>(this IEndpointRouteBuilder app, Func<Type, bool>? predicate = null) where T : class
        => MapEndpoints(app, typeof(T).Assembly, predicate);

    public static void MapEndpoints(this IEndpointRouteBuilder app, Assembly assembly, Func<Type, bool>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(assembly);

        var endpointRouteHandlerInterfaceType = typeof(IEndpointRouteHandler);

        var endpointRouteHandlerTypes = assembly.GetTypes().Where(t =>
            t.IsClass && !t.IsAbstract && !t.IsGenericType
            && t.GetConstructor(Type.EmptyTypes) != null
            && endpointRouteHandlerInterfaceType.IsAssignableFrom(t)
            && (predicate?.Invoke(t) ?? true));

        foreach (var endpointRouteHandlerType in endpointRouteHandlerTypes)
        {
            var instantiatedType = (IEndpointRouteHandler)
                Activator.CreateInstance(endpointRouteHandlerType)!;
            instantiatedType.Map(app);
        }
    }
}