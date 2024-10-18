using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MinimalHelpers.OpenApi;

/// <summary>
/// Extension methods for <see cref="RouteHandlerBuilder"/>.
/// </summary>
/// <seealso cref="RouteHandlerBuilder"/>
public static class RouteHandlerBuilderExtensions
{
    /// <summary>
    /// Adds to <see cref="RouteHandlerBuilder"/> the specified list of status codes as <see cref="ProblemDetails"/> responses.
    /// </summary>
    /// <param name="builder">The <see cref="RouteHandlerBuilder"/>.</param>
    /// <param name="statusCodes">The list of status codes to be added as <see cref="ProblemDetails"/> responses.</param>
    /// <returns>The <see cref="RouteHandlerBuilder"/> with the new status codes responses.</returns>
    public static RouteHandlerBuilder ProducesDefaultProblem(this RouteHandlerBuilder builder, params int[] statusCodes)
    {
        foreach (var statusCode in statusCodes)
        {
            builder.ProducesProblem(statusCode);
        }

        return builder;
    }
}
