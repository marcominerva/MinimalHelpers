# Minimal APIs Helpers

[![Lint Code Base](https://github.com/marcominerva/MinimalHelpers.Routing/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/MinimalHelpers.Routing/actions/workflows/linter.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/MinimalHelpers.Routing/blob/master/LICENSE)

A collection of helpers libraries for Minimal API projects.

## MinimalHelpers.Routing

[![Nuget](https://img.shields.io/nuget/v/MinimalHelpers.Routing)](https://www.nuget.org/packages/MinimalHelpers.Routing)
[![Nuget](https://img.shields.io/nuget/dt/MinimalHelpers.Routing)](https://www.nuget.org/packages/MinimalHelpers.Routing)

A library that provides Routing helpers for Minimal API projects, mainly for automatic endpoints registration.

**Installation**

The library is available on [NuGet](https://www.nuget.org/packages/MinimalHelpers.Routing). Just search for *MinimalHelpers.Routing* in the **Package Manager GUI** or run the following command in the **.NET CLI**:

    dotnet add package MinimalHelpers.Routing

**Usage**

***Automatic Route Endpoints registration***

Create a class to hold your route handlers and make it implementing the `IEndpointRouteHandler` interface:

    public class PeopleHandler : MinimalHelpers.Routing.IEndpointRouteHandler
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

Call the `MapEndpoints()` extension method on the **WebApplication** object inside *Program.cs* before the `Run()` method invocation:

    // using MinimalHelpers.Routing;
    app.MapEndpoints();

    app.Run();

By default, `MapEndpoints()` will scan the calling Assembly to search for classes that implement the `IEndpointRouteHandler` interface. If your route handlers are defined in another Assembly, you have two alternatives:

- Use the `MapEndpoints()` overload that takes the Assembly to scan as argument
- Use the `MapEndpointsFromAssemblyContaining<T>()` extension method and specify a type that is contained in the Assembly you want to scan

You can also explicitly decide what types (among the ones that implement the `IRouteEndpointHandler` interface) you want to actually map, passing a predicate to the `MapEndpoints` method:

    app.MapEndpoints(type =>
    {
        if (type.Name.StartsWith("Products"))
        {
            return false;
        }

        return true;
    });

## MinimalHelpers.OpenApi

[![Nuget](https://img.shields.io/nuget/v/MinimalHelpers.OpenApi)](https://www.nuget.org/packages/MinimalHelpers.OpenApi)
[![Nuget](https://img.shields.io/nuget/dt/MinimalHelpers.OpenApi)](https://www.nuget.org/packages/MinimalHelpers.OpenApi)

A library that provides OpenApi helpers for Minimal API projects.

**Installation**

The library is available on [NuGet](https://www.nuget.org/packages/MinimalHelpers.OpenApi). Just search for *MinimalHelpers.OpenApi* in the **Package Manager GUI** or run the following command in the **.NET CLI**:

    dotnet add package MinimalHelpers.OpenApi

**Usage**

***Add missing schema in Swagger documentation***

Unlike Controllers, current version of Minimal API does not generate the correct schema in Swagger documentation for certain file types, like `Guid`, `DateTime`, `DateOnly` and `TimeOnly`. For example, given the following endpoint:

    app.MapGet("/api/schemas",
        (Guid id, DateTime dateTime, DateOnly date, TimeOnly time) => TypedResults.NoContent());

**swagger.json** will not contain **format** specification for these data types (whereas Controllers correctly set them):

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

To solve the issue, just call the following extension method:

    builder.Services.AddSwaggerGen(options =>
    {
        // using MinimalHelpers.OpenApi;
        options.AddMissingSchemas();
    });

And you'll see that the correct **format** attribute has been specified for each parameter.

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

**Contribute**

The project is constantly evolving. Contributions are welcome. Feel free to file issues and pull requests on the repo and we'll address them as we can. 
