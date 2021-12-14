using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace MinimalHelpers.Registration;

/// <summary>
/// Provides extension methods for <see cref="IEndpointRouteHandler" /> to add route handlers.
/// </summary>
/// <seealso cref="IEndpointRouteBuilder" />
public static class IEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Scans the calling <see cref="Assembly"/> to search for classes that implement the <see cref="IEndpointRouteHandler "/> interface and automatically register all their route endpoints.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder" /> to add routes to.</param>
    /// <param name="predicate">A function to test each class type for a condition.</param>    
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints, Func<Type, bool>? predicate = null)
        => MapEndpoints(endpoints, Assembly.GetCallingAssembly(), predicate);

    /// <summary>
    /// Scans the specified <see cref="Assembly"/> to search for classes that implement the <see cref="IEndpointRouteHandler "/> interface and automatically register all their route endpoints.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder" /> to add routes to.</param>
    /// <param name="assembly">The <see cref="Assembly"/> to scan.</param>
    /// <param name="predicate">A function to test each class type for a condition.</param>   
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints, Assembly assembly, Func<Type, bool>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        ArgumentNullException.ThrowIfNull(assembly);

        var endpointRouteHandlerInterfaceType = typeof(IEndpointRouteHandler);

        var endpointRouteHandlerTypes = assembly.GetTypes().Where(t =>
            t.IsClass && !t.IsAbstract && !t.IsGenericType
            && t.GetConstructor(Type.EmptyTypes) != null
            && endpointRouteHandlerInterfaceType.IsAssignableFrom(t)
            && (predicate?.Invoke(t) ?? true));

        foreach (var endpointRouteHandlerType in endpointRouteHandlerTypes)
        {
            var instantiatedType = (IEndpointRouteHandler)Activator.CreateInstance(endpointRouteHandlerType)!;
            instantiatedType.Map(endpoints);
        }
    }

    /// <summary>
    /// Scans the <see cref="Assembly"/> that contains the specified type to search for classes that implement the <see cref="IEndpointRouteHandler "/> interface and automatically register all their route endpoints.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder" /> to add routes to.</param>
    /// <param name="predicate">A function to test each class type for a condition.</param>   
    public static void MapEndpointsFromAssemblyContaining<T>(this IEndpointRouteBuilder endpoints, Func<Type, bool>? predicate = null) where T : class
        => MapEndpoints(endpoints, typeof(T).Assembly, predicate);
}