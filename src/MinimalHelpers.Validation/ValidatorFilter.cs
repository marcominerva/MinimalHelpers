using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MiniValidation;

namespace MinimalHelpers.Validation;

internal class ValidatorFilter<T>(IOptions<ValidationOptions> options) : IEndpointFilter where T : class
{
    private readonly ValidationOptions validationOptions = options.Value;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(a => a?.GetType() == typeof(T)) is not T input)
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
