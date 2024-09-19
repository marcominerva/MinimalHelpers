using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MinimalHelpers.Validation;

/// <summary>
/// Options for configuring validation behavior.
/// </summary>
/// <seealso cref="ProblemDetails"/>"/>
public class ValidationOptions
{
    /// <summary>
    /// Gets or sets the format of the error messages in the <see cref="ProblemDetails"/> response.
    /// </summary>
    /// <seealso cref="ErrorResponseFormat"/>
    /// <seealso cref="ProblemDetails"/>"/>
    public ErrorResponseFormat ErrorResponseFormat { get; set; }

    /// <summary>
    /// Gets or sets the factory for creating the title property in the <see cref="ProblemDetails"/> validation error messages.
    /// </summary>
    /// <seealso cref="ProblemDetails"/>
    public Func<EndpointFilterInvocationContext, IDictionary<string, string[]>, string>? ValidationErrorMessageFactory { get; set; }
}
