using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MinimalHelpers.OpenApi;

/// <summary>
/// Provides extension methods for <see cref="SwaggerGenOptions"/>.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Adds an <see cref="IOperationFilter"/> that fixes <see langword="swagger.json"/> generation for <see cref="IFormFile"/> and <see cref="IFormFileCollection"/> parameters when using the <see langword="WithOpenApi"/> extension method on endpoints.
    /// </summary>
    /// <param name="options">The <see cref="SwaggerGenOptions"/> that contains the options to generate Swagger documentation.</param>
    /// <seealso cref="IFormFile"/>
    /// <seealso cref="IFormFileCollection"/>
    /// <seealso cref="SwaggerGenOptions"/>
    /// <seealso cref="IOperationFilter"/>
    [Obsolete("This method is no longer necessary and will be removed in a future version")]
    public static void AddFormFile(this SwaggerGenOptions options)
    {
    }

    /// <summary>
    /// Gets the <see cref="OpenApiParameter"/> by name from the specified list of parameters.
    /// </summary>
    /// <param name="parameters">The list of <see cref="OpenApiParameter"/> objects.</param>
    /// <param name="name">The name of the parameter to retrieve.</param>
    /// <returns>The <see cref="OpenApiParameter"/> object with the specified name.</returns>
    /// <exception cref="InvalidOperationException">The parameter with the specified name was not found.</exception>    
    public static OpenApiParameter GetByName(this IList<OpenApiParameter> parameters, string name)
        => parameters.Single(p => p.Name == name);

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
