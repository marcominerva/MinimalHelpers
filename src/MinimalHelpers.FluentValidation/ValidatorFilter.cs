using System.Diagnostics;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MinimalHelpers.Validation;

namespace MinimalHelpers.FluentValidation;

internal class ValidatorFilter<T>(IValidator<T> validator, IOptions<ValidationOptions> options) : IEndpointFilter where T : class
{
    private readonly ValidationOptions validationOptions = options.Value;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(a => a?.GetType() == typeof(T)) is not T input)
        {
            return TypedResults.BadRequest();
        }

        var validationResult = await validator.ValidateAsync(input);

        if (validationResult.IsValid)
        {
            return await next(context);
        }

        var errors = validationResult.ToDictionary();

        var result = TypedResults.ValidationProblem(
            errors: new Dictionary<string, string[]>(),
            instance: context.HttpContext.Request.Path,
            title: validationOptions.ValidationErrorTitleMessageFactory?.Invoke(context, errors) ?? "One or more validation errors occurred",
            extensions: new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier,
                ["errors"] = validationOptions.ErrorResponseFormat == ErrorResponseFormat.Default ? errors : errors.SelectMany(e => e.Value.Select(m => new { Name = e.Key, Message = m })).ToArray()
            }
        );

        return result;

        //var statusCode = StatusCodes.Status400BadRequest;
        //var problemDetails = new ProblemDetails
        //{
        //    Status = statusCode,
        //    Type = $"https://httpstatuses.io/{statusCode}",
        //    Title = validationOptions.ValidationErrorTitleMessageFactory?.Invoke(context, errors) ?? "One or more validation errors occurred",
        //    Instance = context.HttpContext.Request.Path,
        //    Extensions = new Dictionary<string, object?>(StringComparer.Ordinal)
        //    {
        //        ["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier,
        //        ["errors"] = validationOptions.ErrorResponseFormat == ErrorResponseFormat.Default ? errors : errors.SelectMany(e => e.Value.Select(m => new { Name = e.Key, Message = m })).ToArray()
        //    }
        //};

        //return TypedResults.Json(problemDetails, statusCode: statusCode, contentType: "application/problem+json; charset=utf-8");
    }
}
