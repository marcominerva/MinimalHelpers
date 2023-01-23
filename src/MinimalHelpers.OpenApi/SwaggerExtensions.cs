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
    /// Adds an <see cref="IOperationFilter"/> that extends Swagger generation with missing schemas for common types (<c>Guid</c>, <c>DateTime</c>, <c>DateOnly</c> and <c>TimeOnly</c>).
    /// </summary>
    /// <param name="options">The <see cref="SwaggerGenOptions"/> that contains the options to generate Swagger documentation.</param>
    public static void AddMissingSchemas(this SwaggerGenOptions options)
        => options.OperationFilter<MissingSchemasOperationFilter>();
}
