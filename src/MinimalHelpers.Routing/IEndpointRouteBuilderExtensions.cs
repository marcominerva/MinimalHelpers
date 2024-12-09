using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace MinimalHelpers.Routing;

/// <summary>
/// Provides extension methods for <see cref="IEndpointRouteHandlerBuilder" /> to add route handlers.
/// </summary>
/// <seealso cref="IEndpointRouteBuilder" />
public static class IEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Scans the calling <see cref="Assembly"/> to search for classes that implement the <see cref="IEndpointRouteHandlerBuilder "/> interface and automatically register all their route endpoints.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder" /> to add routes to.</param>
    /// <param name="predicate">A function to test each class type for a condition.</param>
    /// <seealso cref="IEndpointRouteBuilder" />
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints, Func<Type, bool>? predicate = null)
        => MapEndpoints(endpoints, Assembly.GetCallingAssembly(), predicate);

    /// <summary>
    /// Scans the specified <see cref="Assembly"/> to search for classes that implement the <see cref="IEndpointRouteHandlerBuilder "/> interface and automatically register all their route endpoints.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder" /> to add routes to.</param>
    /// <param name="assembly">The <see cref="Assembly"/> to scan.</param>
    /// <param name="predicate">A function to test each class type for a condition.</param>   
    /// <seealso cref="IEndpointRouteBuilder" />
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints, Assembly assembly, Func<Type, bool>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        ArgumentNullException.ThrowIfNull(assembly);

        var endpointRouteHandlerBuilderInterfaceType = typeof(IEndpointRouteHandlerBuilder);

        var endpointRouteHandlerBuilderTypes = assembly.GetTypes().Where(t =>
            t.IsClass && !t.IsAbstract && !t.IsGenericType
            && endpointRouteHandlerBuilderInterfaceType.IsAssignableFrom(t)
            && (predicate?.Invoke(t) ?? true));

        foreach (var endpointRouteHandlerBuilderType in endpointRouteHandlerBuilderTypes)
        {
            var mapEndpointsMethod = endpointRouteHandlerBuilderType.GetMethod(nameof(IEndpointRouteHandlerBuilder.MapEndpoints), BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)!;
            mapEndpointsMethod.Invoke(null, [endpoints]);
        }
    }

    /// <summary>
    /// Scans the <see cref="Assembly"/> that contains the specified type to search for classes that implement the <see cref="IEndpointRouteHandlerBuilder "/> interface and automatically register all their route endpoints.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder" /> to add routes to.</param>
    /// <param name="predicate">A function to test each class type for a condition.</param>   
    /// <seealso cref="IEndpointRouteBuilder" />
    public static void MapEndpointsFromAssemblyContaining<T>(this IEndpointRouteBuilder endpoints, Func<Type, bool>? predicate = null) where T : class
        => MapEndpoints(endpoints, typeof(T).Assembly, predicate);

    /// <summary>
    /// Registers all the route endpoints from the specified <see cref="IEndpointRouteHandlerBuilder"/> type.
    /// </summary>
    /// <typeparam name="T">The type that contains the endpoint registrations.</typeparam>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder" /> to add routes to.</param>
    /// <seealso cref="IEndpointRouteBuilder" />
    public static void MapEndpoints<T>(this IEndpointRouteBuilder endpoints) where T : IEndpointRouteHandlerBuilder
        => T.MapEndpoints(endpoints);
}