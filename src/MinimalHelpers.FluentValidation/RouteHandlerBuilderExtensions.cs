using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MinimalHelpers.FluentValidation;

/// <summary>
/// Extension methods for <see cref="RouteHandlerBuilder"/> to add validation.
/// </summary>
/// <seealso cref="RouteHandlerBuilder"/>
public static class RouteHandlerBuilderExtensions
{
    /// <summary>
    /// Adds to the <see cref="RouteHandlerBuilder"/> a filter that validates the <typeparamref name="T"/> object.
    /// </summary>
    /// <typeparam name="T">The type of the object to validate.</typeparam>
    /// <param name="builder">The <see cref="RouteHandlerBuilder"/> to add validation filter to.</param>
    /// <returns>The <see cref="RouteHandlerBuilder"/> with validation filter added.</returns>
    /// <remarks>The validation is performed using <a href="https://fluentvalidation.net">FluentValidation</a>.</remarks>
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder) where T : class
        => builder.AddEndpointFilter<ValidatorFilter<T>>();
}
