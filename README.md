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

## MinimalHelpers.Routing.Analyzers (.NET 7.0 or higher)

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

***Extension methods for OpenApi***

This library provides some extensions methods that simplify the OpenAPI configuration in Minimal API projects. For example, it is possible to customize the description of a response using its status code:

```csharp
endpoints.MapPost("login", LoginAsync)
    .AllowAnonymous()
    .WithValidation<LoginRequest>()
    .Produces<LoginResponse>(StatusCodes.Status200OK)
    .Produces<LoginResponse>(StatusCodes.Status206PartialContent)
    .Produces(StatusCodes.Status403Forbidden)
    .ProducesValidationProblem()
    .WithOpenApi(operation =>
    {
        operation.Summary = "Performs the login of a user";

        operation.Response(StatusCodes.Status200OK).Description = "Login successful";
        operation.Response(StatusCodes.Status206PartialContent).Description = "The user is logged in, but the password has expired and must be changed";
        operation.Response(StatusCodes.Status400BadRequest).Description = "Incorrect username and/or password";
        operation.Response(StatusCodes.Status403Forbidden).Description = "The user was blocked due to too many failed logins";

        return operation;
    });
 ```

**Contribute**

The project is constantly evolving. Contributions are welcome. Feel free to file issues and pull requests on the repo and we'll address them as we can. 
