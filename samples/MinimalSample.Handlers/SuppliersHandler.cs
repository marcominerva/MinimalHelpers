using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MinimalHelpers.Routing;

namespace MinimalSample.Handlers;

public class SuppliersHandler : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/suppliers", GetList);
        endpoints.MapGet("/api/suppliers/{id:guid}", Get);
        endpoints.MapPost("/api/suppliers", Insert).WithOpenApi();
        endpoints.MapPut("/api/suppliers/{id:guid}", Update);
        endpoints.MapDelete("/api/suppliers/{id:guid}", Delete);
    }

    private static IResult GetList() => TypedResults.NoContent();

    private static IResult Get(Guid id) => TypedResults.NoContent();

    private static IResult Insert(Supplier Person, DateTime? creationDate) => TypedResults.NoContent();

    private static IResult Update(Supplier Person) => TypedResults.NoContent();

    private static IResult Delete(Guid id) => TypedResults.NoContent();
}

public record class Supplier(string Name);