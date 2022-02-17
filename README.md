# Minimal APIs Routing Helpers

[![GitHub Super-Linter](https://github.com/marcominerva/MinimalHelpers.Routing/workflows/Lint%20Code%20Base/badge.svg)](https://github.com/marketplace/actions/super-linter)
[![Nuget](https://img.shields.io/nuget/v/MinimalHelpers.Routing)](https://www.nuget.org/packages/MinimalHelpers.Routing)
[![Nuget](https://img.shields.io/nuget/dt/MinimalHelpers.Routing)](https://www.nuget.org/packages/MinimalHelpers.Routing)

A library that provides Routing helpers for Minimal APIs project.

**Installation**

The library is available on [NuGet](https://www.nuget.org/packages/MinimalHelpers.Routing). Just search *MinimalHelpers.Routing* in the **Package Manager GUI** or run the following command in the **Package Manager Console**:

    Install-Package MinimalHelpers.Routing

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

**Contribute**

The project is constantly evolving. Contributions are welcome. Feel free to file issues and pull requests on the repo and we'll address them as we can. 
