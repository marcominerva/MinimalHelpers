using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MinimalHelpers.OpenApi.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MinimalHelpers.OpenApi;

/// <summary>
/// Provides extension methods for <see cref="SwaggerGenOptions"/>.
/// </summary>
public static class SwaggerExtensions
{
#if NET7_0
    /// <summary>
    /// Adds an <see cref="IOperationFilter"/> that extends <see langword="swagger.json"/> generation with missing schemas for common types (<see langword="Guid"/>, <see langword="DateTime"/>, <see langword="DateOnly"/> and <see langword="TimeOnly"/>) when using the <see langword="WithOpenApi"/> extension method on endpoints.
    /// </summary>
    /// <param name="options">The <see cref="SwaggerGenOptions"/> that contains the options to generate Swagger documentation.</param>
    /// <seealso cref="SwaggerGenOptions"/>
    /// <seealso cref="IOperationFilter"/>
    public static void AddMissingSchemas(this SwaggerGenOptions options)
        => options.OperationFilter<MissingSchemasOperationFilter>();
#endif

    /// <summary>
    /// Adds an <see cref="IOperationFilter"/> that fixes <see langword="swagger.json"/> generation for <see cref="IFormFile"/> and <see cref="IFormFileCollection"/> parameters when using the <see langword="WithOpenApi"/> extension method on endpoints.
    /// </summary>
    /// <param name="options">The <see cref="SwaggerGenOptions"/> that contains the options to generate Swagger documentation.</param>
    /// <seealso cref="IFormFile"/>
    /// <seealso cref="IFormFileCollection"/>
    /// <seealso cref="SwaggerGenOptions"/>
    /// <seealso cref="IOperationFilter"/>
    public static void AddFormFile(this SwaggerGenOptions options)
        => options.OperationFilter<FormFileOperationFilter>();
}
