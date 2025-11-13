using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MinimalHelpers.Validation;

/// <summary>
/// Extension methods for <see cref="RouteHandlerBuilder"/> to add validation to Minimal API endpoints.
/// </summary>
/// <remarks>
/// These methods allow you to easily add validation filters to your Minimal API routes. See <see href="https://github.com/marcominerva/MinimalHelpers">project documentation</see> for more details.
/// </remarks>
/// <seealso cref="RouteHandlerBuilder"/>
public static class RouteHandlerBuilderExtensions
{
    /// <summary>
    /// Registers a <see cref="ValidatorFilter{TModel}"/> of type <typeparamref name="TModel"/> onto the route handler to validate the <typeparamref name="TModel"/> object.
    /// </summary>
    /// <typeparam name="TModel">The type of the object to validate.</typeparam>
    /// <param name="builder">The <see cref="RouteHandlerBuilder"/> to add the validation filter to.</param>
    /// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the route handler.</returns>
    /// <remarks>
    /// The validation is performed with Data Annotations, using <see href="https://github.com/DamianEdwards/MiniValidation">MiniValidation</see>.
    /// <example>
    /// <code language="csharp">
    /// app.MapPost("/api/products", (Product product) =>
    /// {
    ///     // ...
    /// })
    /// .WithValidation&lt;Product&gt;();
    /// </code>
    /// </example>
    /// </remarks>
    /// <seealso cref="ValidatorFilter{TModel}"/>
#if NET10_0_OR_GREATER
    [Obsolete("This method is obsolete starting from .NET 10. Consider using built-in validation features of .NET 10 and later.")]
#endif
    public static RouteHandlerBuilder WithValidation<TModel>(this RouteHandlerBuilder builder) where TModel : class
    {
        builder.AddEndpointFilter<ValidatorFilter<TModel>>()
            .ProducesValidationProblem();

        return builder;
    }
}
