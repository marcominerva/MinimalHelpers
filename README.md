# Minimal APIs Endpoints Registration Helper

[![GitHub Super-Linter](https://github.com/marcominerva/MinimalHelpers.Registration/workflows/Lint%20Code%20Base/badge.svg)](https://github.com/marketplace/actions/super-linter)
[![Nuget](https://img.shields.io/nuget/v/MinimalHelpers.Registration)](https://www.nuget.org/packages/MinimalHelpers.Registration)
[![Nuget](https://img.shields.io/nuget/dt/MinimalHelpers.Registration)](https://www.nuget.org/packages/MinimalHelpers.Registration)

A lightweight library to automatically register all the Route Endpoints of a Minimal API project.

**Installation**

The library is available on [NuGet](https://www.nuget.org/packages/MinimalHelpers.Registration). Just search *MinimalHelpers.Registration* in the **Package Manager GUI** or run the following command in the **Package Manager Console**:

    Install-Package MinimalHelpers.Registration

**Usage**

Create a class to hold your route handlers and make it implementing the `IEndpointRouteHandler` interface:

    public class PeopleHandler : MinimalHelpers.Registration.IEndpointRouteHandler
    {
        public void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/people", GetList);
            app.MapGet("/api/people/{id:guid}", Get);
            app.MapPost("/api/people", Insert);
            app.MapPut("/api/people/{id:guid}", Update);
            app.MapDelete("/api/people/{id:guid}", Delete);
        }

        // ...
    }

Call the `MapEndpoints()` extension method on the **WebApplication** object inside *Program.cs* before the `Run()` method invocation:

    // using MinimalHelpers.Registration;
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
