using System.Diagnostics;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MinimalHelpers.Validation;

namespace MinimalHelpers.FluentValidation;

/// <summary>
/// Endpoint filter that validates a model of type <typeparamref name="TModel"/> using FluentValidation.
/// </summary>
/// <typeparam name="TModel">The type of the model to validate.</typeparam>
/// <remarks>
/// This filter is used internally by <see cref="RouteHandlerBuilderExtensions.WithValidation{T}"/>. It uses <see href="https://fluentvalidation.net">FluentValidation</see> to perform validation.
/// </remarks>
internal class ValidatorFilter<TModel>(IValidator<TModel> validator, IOptions<ValidationOptions> options) : IEndpointFilter where TModel : class
{
    private readonly ValidationOptions validationOptions = options.Value;

    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(a => a?.GetType() == typeof(TModel)) is not TModel input)
        {
            return TypedResults.BadRequest();
        }

        var validationResult = await validator.ValidateAsync(input, context.HttpContext.RequestAborted);

        if (validationResult.IsValid)
        {
            return await next(context);
        }

        var errors = validationResult.ToDictionary();

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
