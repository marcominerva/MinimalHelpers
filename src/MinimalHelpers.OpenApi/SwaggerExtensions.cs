﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
    /// Gets the <see cref="OpenApiParameter"/> by name that is available in the <see cref="OpenApiOperation"/> paramters list.
    /// </summary>
    /// <param name="operation">The <see cref="OpenApiOperation"/> object.</param>
    /// <param name="name">The name of the parameter to retrieve.</param>
    /// <returns>The <see cref="OpenApiParameter"/> object with the specified name.</returns>
    /// <exception cref="InvalidOperationException">The parameter with the specified name was not found.</exception>
    public static OpenApiParameter Parameter(this OpenApiOperation operation, string name)
        => operation.Parameters.Single(p => p.Name == name);

}
