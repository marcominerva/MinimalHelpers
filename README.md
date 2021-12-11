# Minimal APIs Endpoints registration helper

[![GitHub Super-Linter](https://github.com/marcominerva/MinimalHelpers.Registration/workflows/Lint%20Code%20Base/badge.svg)](https://github.com/marketplace/actions/super-linter)
[![NuGet version (TinyHelpers)](https://img.shields.io/nuget/v/MinimalHelpers.Registration.svg?style=flat-square)](https://www.nuget.org/packages/MinimalHelpers.Registration)

A lightweight library to automatically register all the Route Endpoints of a Minimal API project.

**Installation**

The library is available on [NuGet](https://www.nuget.org/packages/MinimalHelpers.Registration). Just search *MinimalHelpers.Registration* in the **Package Manager GUI** or run the following command in the **Package Manager Console**:   

    Install-Package MinimalHelpers.Registration

**Usage**

Create a class to hold your route handlers and make it implementing the `IRouteEndpointHandler` interface:

    public class PeopleHandler : MinimalHelpers.Registration.IRouteEndpointHandler
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

By default, `MapEnpoints()` will scan the calling Assembly to search for classes that implement the `IRouteEndpointHandler` interface. If your route handlers are defined in another Assembly, you have two alternatives:

- use the `MapEndpoints()` overload that takes the Assembly to scan as argument
- Use the `MapEndpointsFromAssemblyContaining<T>()` extension method and specify a type that is contained in the Assembly you want to scan

**Contribute**

The project is continually evolving. We welcome contributions. Feel free to file issues and pull requests on the repo and we'll address them as we can. 
