# Minimal APIs Helpers

[![Lint Code Base](https://github.com/marcominerva/MinimalHelpers/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/MinimalHelpers/actions/workflows/linter.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/MinimalHelpers/blob/master/LICENSE)

A collection of helpers libraries for Minimal API projects.

## MinimalHelpers.Routing

[![Nuget](https://img.shields.io/nuget/v/MinimalHelpers.Routing)](https://www.nuget.org/packages/MinimalHelpers.Routing)
[![Nuget](https://img.shields.io/nuget/dt/MinimalHelpers.Routing)](https://www.nuget.org/packages/MinimalHelpers.Routing)

A library that provides Routing helpers for Minimal API projects for automatic endpoints registration using Reflection.

### Installation

The library is available on [NuGet](https://www.nuget.org/packages/MinimalHelpers.Routing). Just search for *MinimalHelpers.Routing* in the **Package Manager GUI** or run the following command in the **.NET CLI**:

```shell
dotnet add package MinimalHelpers.Routing
```

### Usage

Create a class to hold your route handlers registration and make it implementing the `IEndpointRouteHandlerBuilder` interface:

**.NET 6.0**

```csharp
public class PeopleHandler : MinimalHelpers.Routing.IEndpointRouteHandlerBuilder
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/people", GetList);
        endpoints.MapGet("/api/people/{id:guid}", Get);
        endpoints.MapPost("/api/people", Insert);
        endpoints.MapPut("/api/people/{id:guid}", Update);
        endpoints.MapDelete("/api/people/{id:guid}", Delete);
    }

    // ...
}
```

**.NET 7.0 or higher**

```csharp
public class PeopleHandler : MinimalHelpers.Routing.IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/people", GetList);
        endpoints.MapGet("/api/people/{id:guid}", Get);
        endpoints.MapPost("/api/people", Insert);
        endpoints.MapPut("/api/people/{id:guid}", Update);
        endpoints.MapDelete("/api/people/{id:guid}", Delete);
    }

    // ...
}
```

> **Note**
Starting from .NET 7.0, the `IEndpointRouteHandlerBuilder` interface exposes the `MapEndpoints` method as static abstract, so it can be called without creating an instance of the handler.

Call the `MapEndpoints()` extension method on the **WebApplication** object inside *Program.cs* before the `Run()` method invocation:

```csharp
// using MinimalHelpers.Routing;
app.MapEndpoints();

app.Run();
```

By default, `MapEndpoints()` will scan the calling Assembly to search for classes that implement the `IEndpointRouteHandlerBuilder` interface. If your route handlers are defined in another Assembly, you have two alternatives:

- Use the `MapEndpoints()` overload that takes the Assembly to scan as argument
- Use the `MapEndpointsFromAssemblyContaining<T>()` extension method and specify a type that is contained in the Assembly you want to scan

You can also explicitly decide what types (among the ones that implement the `IRouteEndpointHandlerBuilder` interface) you want to actually map, passing a predicate to the `MapEndpoints` method:

```csharp
app.MapEndpoints(type =>
{
    if (type.Name.StartsWith("Products"))
    {
        return false;
    }

    return true;
});
```

> **Note**
These methods rely on Reflection to scan the Assembly and find the classes that implement the `IEndpointRouteHandlerBuilder` interface. This can have a performance impact, especially in large projects. If you have performance issues, consider using the explicit registration method. Moreover, this solution is incompatibile with Native AOT.

If you're working with .NET 7.0 or higher, the reccommended approach is to use the **MinimalHelpers.Routing.Analyzers** package, that provides a Source Generator for endpoints registration, as described later.

## MinimalHelpers.Routing.Analyzers

[![Nuget](https://img.shields.io/nuget/v/MinimalHelpers.Routing.Analyzers)](https://www.nuget.org/packages/MinimalHelpers.Routing.Analyzers)
[![Nuget](https://img.shields.io/nuget/dt/MinimalHelpers.Routing.Analyzers)](https://www.nuget.org/packages/MinimalHelpers.Routing.Analyzers)

A library that provides a Source Generator for automatic endpoints registration in Minimal API projects.

### Installation

The library is available on [NuGet](https://www.nuget.org/packages/MinimalHelpers.Routing.Analyzers). Just search for *MinimalHelpers.Routing* in the **Package Manager GUI** or run the following command in the **.NET CLI**:

```shell
dotnet add package MinimalHelpers.Routing.Analyzers
```

### Usage

Create a class to hold your route handlers registration and make it implementing the `IEndpointRouteHandlerBuilder` interface:

```csharp
public class PeopleHandler : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/people", GetList);
        endpoints.MapGet("/api/people/{id:guid}", Get);
        endpoints.MapPost("/api/people", Insert);
        endpoints.MapPut("/api/people/{id:guid}", Update);
        endpoints.MapDelete("/api/people/{id:guid}", Delete);
    }

    // ...
}
```

> **Note**
You only need to use the **MinimalHelpers.Routing.Analyzers** package. With this Source Generator, the `IEndpointRouteHandlerBuilder` interface is auto-generated.

Call the `MapEndpoints()` extension method on the **WebApplication** object inside *Program.cs* before the `Run()` method invocation:

```csharp
app.MapEndpoints();

app.Run();
```

> **Note**
The `MapEndpoints` method is generated by the Source Generator.

## MinimalHelpers.OpenApi

[![Nuget](https://img.shields.io/nuget/v/MinimalHelpers.OpenApi)](https://www.nuget.org/packages/MinimalHelpers.OpenApi)
[![Nuget](https://img.shields.io/nuget/dt/MinimalHelpers.OpenApi)](https://www.nuget.org/packages/MinimalHelpers.OpenApi)

A library that provides OpenApi helpers for Minimal API projects.

### Installation

The library is available on [NuGet](https://www.nuget.org/packages/MinimalHelpers.OpenApi). Just search for *MinimalHelpers.OpenApi* in the **Package Manager GUI** or run the following command in the **.NET CLI**:

```shell
dotnet add package MinimalHelpers.OpenApi
```

### Usage

***Add OpenApi support for IFormFile and IFormFileCollection***

Minimal APIs don't generate the correct schema in **swagger.json** if we have an endpoint that accepts a [IFormFile](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.iformfile) or [IFormFileCollection](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.iformfilecollection) parameter and we're using the [WithOpenApi](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.openapiendpointconventionbuilderextensions.withopenapi) extension method in .NET 7.0 or later. For example:

```csharp
app.MapPost("/api/upload", (IFormFile file) =>
{
    return TypedResults.Ok(new { file.FileName, file.ContentType, file.Length });
})
.WithOpenApi();
```

This definition generates the following incorrect content in **swagger.json**:

```json
"requestBody": {
    "content": {
        "multipart/form-data": {
            "schema": {
                "type": "string",
                "format": "binary"
            }
        }
    },
    "required": true
}
```

To solve this issue, just call the following extension method:

```csharp
builder.Services.AddSwaggerGen(options =>
{
    // using MinimalHelpers.OpenApi;
    options.AddFormFile();
});
```

And now the [IFormFile](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.iformfile) is correctly defined:

```json
"requestBody": {
  "content": {
    "multipart/form-data": {
      "schema": {
        "required": [
          "file"
        ],
        "type": "object",
        "properties": {
          "file": {
            "type": "string",
            "format": "binary"
          }
        }
      },
      "encoding": {
        "file": {
          "style": "form"
        }
      }
    }
  }
}
```

***Add missing schema in swagger.json (.NET 7.0)***

Minimal APIs in .NET 7.0 don't generate the correct schema in **swagger.json** for certain file types, like `Guid`, `DateTime`, `DateOnly` and `TimeOnly` when using the [WithOpenApi](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.openapiendpointconventionbuilderextensions.withopenapi) extension method on endpoints. For example, given the following endpoint:

```csharp
    app.MapGet("/api/schemas",
        (Guid id, DateTime dateTime, DateOnly date, TimeOnly time) => TypedResults.NoContent());
```

**swagger.json** will not contain **format** specification for these data types (whereas Controllers correctly set them):

```json
"parameters": [
  {
    "name": "id",
    // ...
    "schema": {
      "type": "string"
    }
  },
  {
    "name": "dateTime",
    // ...
    "schema": {
      "type": "string"
    }
  },
  {
    "name": "date",
    // ...
    "schema": {
      "type": "string"
    }
  },
  {
    "name": "time",
    // ...
    "schema": {
      "type": "string"
    }
  }
]
```

To solve these issues, just call the following extension method:

```csharp
builder.Services.AddSwaggerGen(options =>
{
    // using MinimalHelpers.OpenApi;
    options.AddMissingSchemas();
});
```

And you'll see that the correct **format** attribute has been specified for each parameter.

```json
"parameters": [
  {
    "name": "id",
    // ...
    "schema": {
      "type": "string",
      "format": "uuid"
    }
  },
  {
    "name": "dateTime",
    // ...
    "schema": {
      "type": "string",
      "format": "date-time"
    }
  },
  {
    "name": "date",
    // ...
    "schema": {
      "type": "string",
      "format": "date"
    }
  },
  {
    "name": "time",
    // ...
    "schema": {
      "type": "string",
      "format": "time"
    }
  }
]    
```

> **Note**
This workaround is no longer necessary in .NET 8.0 or higher, since it correctly sets in the **format** attribute in **swagger.json** for these data types.


**Contribute**

The project is constantly evolving. Contributions are welcome. Feel free to file issues and pull requests on the repo and we'll address them as we can. 
