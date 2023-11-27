using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MinimalHelpers.OpenApi.Filters;

internal class FormFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ParameterDescriptions is not null)
        {
            foreach (var parameterDescription in context.ApiDescription.ParameterDescriptions.Where(p => p.Type == typeof(IFormFile) || p.Type == typeof(IFormFileCollection)))
            {
                var schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        [parameterDescription.Name] = parameterDescription.Type == typeof(IFormFile) ?
                            new() { Type = "string", Format = "binary" }
                            : new() { Type = "array", Items = new() { Type = "string", Format = "binary" } }
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
                        ["multipart/form-data"] = new()
                        {
                            Schema = schema,
                            Encoding = new Dictionary<string, OpenApiEncoding>
                            {
                                [parameterDescription.Name] = new() { Style = ParameterStyle.Form }
                            }
                        }
                    }
                };
            }
        }
    }
}

