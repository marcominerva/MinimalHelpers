using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        var statusCode = StatusCodes.Status400BadRequest;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Type = $"https://httpstatuses.io/{statusCode}",
            Title = validationOptions.ValidationErrorTitleMessageFactory?.Invoke(context, errors) ?? "One or more validation errors occurred",
            Instance = context.HttpContext.Request.Path,
            Extensions = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier,
                ["errors"] = validationOptions.ErrorResponseFormat == ErrorResponseFormat.Default ? errors : errors.SelectMany(e => e.Value.Select(m => new { Name = e.Key, Message = m })).ToArray()
            }
        };

        return TypedResults.Json(problemDetails, statusCode: statusCode, contentType: "application/problem+json; charset=utf-8");
    }
}
