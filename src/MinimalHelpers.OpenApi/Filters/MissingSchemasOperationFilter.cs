using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MinimalHelpers.OpenApi.Filters;

internal class MissingSchemasOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ParameterDescriptions is not null)
        {
            foreach (var parameterDescription in context.ApiDescription.ParameterDescriptions)
            {
                var schema = GetSchema(parameterDescription);
                if (schema.Format is not null)
                {
                    var parameter = operation.Parameters?.FirstOrDefault(p => p.Name == parameterDescription.Name && p.Schema.Type == "string");
                    if (parameter is not null)
                    {
                        parameter.Schema.Format = schema.Format;
                        parameter.Schema.Example = schema.Example is not null ? new OpenApiString(schema.Example) : null;
                    }
                }
            }
        }

#if NET7_0_OR_GREATER
        // This is a workaround for the issue https://github.com/dotnet/aspnetcore/issues/45365.
        // It will be removed as soon as the fix will be included in the NET 7.0.x release.
        if (operation.Parameters?.Any() ?? false)
        {
            foreach (var parameter in operation.Parameters.Where(p => p.In is ParameterLocation.Query or ParameterLocation.Path or ParameterLocation.Header))
            {
                parameter.Content = null;
            }
        }
#endif

        // We provide a way to return also the example, even if at this moment we actually don't use it.
        static (string? Format, string? Example) GetSchema(ApiParameterDescription parameterDescription)
        {
            string? format = null;
            string? example = null;

            if (parameterDescription.Type == typeof(Guid) || parameterDescription.Type == typeof(Guid?))
            {
                (format, example) = ("uuid", null);
            }
            else if (parameterDescription.Type == typeof(DateTime) || parameterDescription.Type == typeof(DateTime?))
            {
                (format, example) = ("date-time", null);
            }
            else if (parameterDescription.Type == typeof(DateOnly) || parameterDescription.Type == typeof(DateOnly?))
            {
                (format, example) = ("date", null);
            }
            else if (parameterDescription.Type == typeof(TimeOnly) || parameterDescription.Type == typeof(TimeOnly?))
            {
                (format, example) = ("time", null);
            }

            return (format, example);
        }
    }
}

