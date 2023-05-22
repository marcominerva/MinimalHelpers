using Microsoft.AspNetCore.Http;
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
            // IFormFile and IFormFileCollection are special cases and thus must be handled separately.
            foreach (var parameterDescription in context.ApiDescription.ParameterDescriptions)
            {
                if (parameterDescription.Type == typeof(IFormFile) || parameterDescription.Type == typeof(IFormFileCollection))
                {
                    var schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            [parameterDescription.Name] = parameterDescription.Type == typeof(IFormFile) ?
                                new OpenApiSchema { Type = "string", Format = "binary" }
                                : new OpenApiSchema { Type = "array", Items = new OpenApiSchema { Type = "string", Format = "binary" } }
                        }
                    };

                    if (parameterDescription.IsRequired)
                    {
                        schema.Required.Add(parameterDescription.Name);
                    }

                    operation.RequestBody = new OpenApiRequestBody
                    {
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["multipart/form-data"] = new OpenApiMediaType
                            {
                                Schema = schema,
                                Encoding = new Dictionary<string, OpenApiEncoding>
                                {
                                    [parameterDescription.Name] = new OpenApiEncoding { Style = ParameterStyle.Form }
                                }
                            }
                        }
                    };
                }
                else
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
        }

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

