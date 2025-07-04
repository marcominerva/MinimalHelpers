using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MiniValidation;

namespace MinimalHelpers.Validation;

/// <summary>
/// Endpoint filter that validates a model of type <typeparamref name="TModel"/> using Data Annotations.
/// </summary>
/// <typeparam name="TModel">The type of the model to validate.</typeparam>
/// <remarks>
/// This filter is used internally by <see cref="RouteHandlerBuilderExtensions.WithValidation{T}"/>. It uses <see href="https://github.com/DamianEdwards/MiniValidation">MiniValidation</see> to perform validation.
/// </remarks>
internal class ValidatorFilter<TModel>(IOptions<ValidationOptions> options) : IEndpointFilter where TModel : class
{
    private readonly ValidationOptions validationOptions = options.Value;

    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(a => a?.GetType() == typeof(TModel)) is not TModel input)
        {
            return TypedResults.BadRequest();
        }

        var (isValid, errors) = await MiniValidator.TryValidateAsync(input);

        if (isValid)
        {
            return await next(context);
        }

        var result = TypedResults.Problem(
            statusCode: StatusCodes.Status400BadRequest,
            instance: context.HttpContext.Request.Path,
            title: validationOptions.ValidationErrorTitleMessageFactory?.Invoke(context, errors) ?? "One or more validation errors occurred",
            extensions: new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier,
                ["errors"] = validationOptions.ErrorResponseFormat == ErrorResponseFormat.Default ? errors : errors.SelectMany(e => e.Value.Select(m => new { Name = e.Key, Message = m })).ToArray()
            }
        );

        return result;
    }
}
