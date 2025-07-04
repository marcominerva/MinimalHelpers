using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MinimalHelpers.Validation;

/// <summary>
/// Options for configuring validation behavior for Minimal APIs.
/// </summary>
/// <remarks>
/// Use this class to customize how validation errors are returned from Minimal API endpoints. See <see href="https://github.com/marcominerva/MinimalHelpers">project documentation</see> for more details.
/// </remarks>
/// <seealso cref="ProblemDetails"/>
public class ValidationOptions
{
    /// <summary>
    /// Gets or sets the format of the error messages in the <see cref="ProblemDetails"/> response.
    /// </summary>
    /// <remarks>
    /// The default is <see cref="ErrorResponseFormat.Default"/>, which returns errors as a dictionary. Set to <see cref="ErrorResponseFormat.List"/> to return errors as a list.
    /// </remarks>
    /// <seealso cref="ErrorResponseFormat"/>
    public ErrorResponseFormat ErrorResponseFormat { get; set; }

    /// <summary>
    /// Gets or sets the factory for creating the <c>title</c> property in the <see cref="ProblemDetails"/> validation error messages.
    /// </summary>
    /// <remarks>
    /// This can be used to localize or customize the error title. For example:
    /// <code language="csharp">
    /// builder.Services.ConfigureValidation(options =>
    /// {
    ///     options.ValidationErrorTitleMessageFactory = (context, errors) =>
    ///         $"There was {errors.Values.Sum(v => v.Length)} error(s)";
    /// });
    /// </code>
    /// </remarks>
    /// <seealso cref="ProblemDetails"/>
    public Func<EndpointFilterInvocationContext, IDictionary<string, string[]>, string>? ValidationErrorTitleMessageFactory { get; set; }
}
