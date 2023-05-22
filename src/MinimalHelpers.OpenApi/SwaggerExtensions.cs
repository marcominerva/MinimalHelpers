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
    /// <summary>
    /// Adds an <see cref="IOperationFilter"/> that extends Swagger generation with missing schemas for common types (<see langword="Guid"/>, <see langword="DateTime"/>, <see langword="DateOnly"/> and <see langword="TimeOnly"/>) and correctly supports <see cref="IFormFile"/> and <see cref="IFormFileCollection"/> parameters when using the <see langword="WithOpenApi"/> extension method on endpoints.
    /// </summary>
    /// <param name="options">The <see cref="SwaggerGenOptions"/> that contains the options to generate Swagger documentation.</param>    
    public static void AddMissingSchemas(this SwaggerGenOptions options)
        => options.OperationFilter<MissingSchemasOperationFilter>();
}
