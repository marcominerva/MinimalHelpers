using Microsoft.AspNetCore.Routing;

namespace MinimalHelpers.Routing;

/// <summary>
/// Defines a contract for a class that holds one or more route handlers that must be registered by the application.
/// </summary>
/// <seealso cref="IEndpointRouteBuilder" />
/// <seealso cref="IEndpointRouteBuilderExtensions" />
public interface IEndpointRouteHandler
{
    /// <summary>
    /// Maps route endpoints to the corresponding handlers.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder" /> to add routes to.</param>
    public void Map(IEndpointRouteBuilder endpoints);
}
