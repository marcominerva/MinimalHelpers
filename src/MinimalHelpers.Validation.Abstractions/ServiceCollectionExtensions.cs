using Microsoft.Extensions.DependencyInjection;

namespace MinimalHelpers.Validation;

/// <summary>
/// Provides extension methods for adding validation support to Minimal APIs.
/// </summary>
/// <remarks>
/// Use these methods to register validation options and customize error responses for Minimal APIs. See <see href="https://github.com/marcominerva/MinimalHelpers">project documentation</see> for more details.
/// </remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the validation services to the specified <see cref="IServiceCollection"/> and configures validation options.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configureOptions">The <see cref="Action{ValidationOptions}"/> to configure the validation options.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <example>
    /// <code language="csharp">
    /// builder.Services.ConfigureValidation(options =>
    /// {
    ///     options.ErrorResponseFormat = ErrorResponseFormat.List;
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection ConfigureValidation(this IServiceCollection services, Action<ValidationOptions> configureOptions)
    {
        services.Configure(configureOptions);
        return services;
    }
}
