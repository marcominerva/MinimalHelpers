#if NET8_0 || NET9_0

using Microsoft.OpenApi.Models;

namespace MinimalHelpers.OpenApi;

/// <summary>
/// Provides extension methods for OpenAPI objects.
/// </summary>
public static class OpenApiExtensions
{
    /// <summary>
    /// Gets the <see cref="OpenApiParameter"/> by name from the specified list of parameters.
    /// </summary>
    /// <param name="parameters">The list of <see cref="OpenApiParameter"/> objects.</param>
    /// <param name="name">The name of the parameter to retrieve.</param>
    /// <returns>The <see cref="OpenApiParameter"/> object with the specified name.</returns>
    /// <exception cref="InvalidOperationException">The parameter with the specified name was not found.</exception>    
    public static OpenApiParameter GetByName(this IList<OpenApiParameter> parameters, string name)
        => parameters.Single(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Gets by name the <see cref="OpenApiParameter"/> that is available in the <see cref="OpenApiOperation"/> parameters list.
    /// </summary>
    /// <param name="operation">The <see cref="OpenApiOperation"/> object.</param>
    /// <param name="name">The name of the parameter to retrieve.</param>
    /// <returns>The <see cref="OpenApiParameter"/> object with the specified name.</returns>
    /// <exception cref="InvalidOperationException">The parameter with the specified name was not found.</exception>
    public static OpenApiParameter Parameter(this OpenApiOperation operation, string name)
        => operation.Parameters.GetByName(name);

    /// <summary>
    /// Gets the <see cref="OpenApiResponse"/> by status code from the specified list of status codes.
    /// </summary>
    /// <param name="responses">The object that contains the list of <see cref="OpenApiResponse"/> objects.</param>
    /// <param name="statusCode">The status code of the response to retrieve.</param>
    /// <returns>The <see cref="OpenApiResponse"/> object with the specified status code.</returns>
    /// <exception cref="InvalidOperationException">The response with the specified name was not found.</exception>    
    public static OpenApiResponse GetByStatusCode(this OpenApiResponses responses, int statusCode)
        => responses.Single(r => r.Key == statusCode.ToString()).Value;

    /// <summary>
    /// Gets by status code the <see cref="OpenApiResponse"/> that is available in the <see cref="OpenApiOperation"/> responses list.
    /// </summary>
    /// <param name="operation">The <see cref="OpenApiOperation"/> object.</param>
    /// <param name="statusCode">The status code of the response to retrieve.</param>
    /// <returns>The <see cref="OpenApiResponse"/> object with the specified status code.</returns>
    /// <exception cref="InvalidOperationException">The response with the specified name was not found.</exception>    
    public static OpenApiResponse Response(this OpenApiOperation operation, int statusCode)
        => operation.Responses.GetByStatusCode(statusCode);
}

#elif NET10_0_OR_GREATER

using Microsoft.OpenApi;

namespace MinimalHelpers.OpenApi;

/// <summary>
/// Provides extension methods for OpenAPI objects.
/// </summary>
public static class OpenApiExtensions
{
    /// <summary>
    /// Gets the <see cref="IOpenApiParameter"/> by name from the specified list of parameters.
    /// </summary>
    /// <param name="parameters">The list of <see cref="OpenApiParameter"/> objects.</param>
    /// <param name="name">The name of the parameter to retrieve.</param>
    /// <returns>The <see cref="IOpenApiParameter"/> object with the specified name.</returns>
    public static IOpenApiParameter? GetByName(this IList<IOpenApiParameter> parameters, string name)
        => parameters.SingleOrDefault(p => p.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);

    /// <summary>
    /// Gets by name the <see cref="IOpenApiParameter"/> that is available in the <see cref="OpenApiOperation"/> parameters list.
    /// </summary>
    /// <param name="operation">The <see cref="OpenApiOperation"/> object.</param>
    /// <param name="name">The name of the parameter to retrieve.</param>
    /// <returns>The <see cref="IOpenApiParameter"/> object with the specified name.</returns>
    public static IOpenApiParameter? Parameter(this OpenApiOperation operation, string name)
        => operation.Parameters?.GetByName(name);

    /// <summary>
    /// Gets the <see cref="IOpenApiResponse"/> by status code from the specified list of status codes.
    /// </summary>
    /// <param name="responses">The object that contains the list of <see cref="OpenApiResponse"/> objects.</param>
    /// <param name="statusCode">The status code of the response to retrieve.</param>
    /// <returns>The <see cref="IOpenApiResponse"/> object with the specified status code.</returns>
    public static IOpenApiResponse GetByStatusCode(this OpenApiResponses responses, int statusCode)
        => responses.SingleOrDefault(r => r.Key == statusCode.ToString()).Value;

    /// <summary>
    /// Gets by status code the <see cref="IOpenApiResponse"/> that is available in the <see cref="OpenApiOperation"/> responses list.
    /// </summary>
    /// <param name="operation">The <see cref="OpenApiOperation"/> object.</param>
    /// <param name="statusCode">The status code of the response to retrieve.</param>
    /// <returns>The <see cref="IOpenApiResponse"/> object with the specified status code.</returns>
    public static IOpenApiResponse? Response(this OpenApiOperation operation, int statusCode)
        => operation.Responses?.GetByStatusCode(statusCode);
}

#endif