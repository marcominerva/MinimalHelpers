using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MinimalHelpers.Registration;

namespace MinimalSample.Handlers;

public class SuppliersHandler : IEndpointRouteHandler
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/suppliers", GetList);
        endpoints.MapGet("/api/suppliers/{id:guid}", Get);
        endpoints.MapPost("/api/suppliers", Insert);
        endpoints.MapPut("/api/suppliers/{id:guid}", Update);
        endpoints.MapDelete("/api/suppliers/{id:guid}", Delete);
    }

    private static IResult GetList() => Results.NoContent();

    private static IResult Get(Guid id) => Results.NoContent();

    private static IResult Insert(Supplier Person) => Results.NoContent();

    private static IResult Update(Supplier Person) => Results.NoContent();

    private static IResult Delete(Guid id) => Results.NoContent();
}

public record Supplier(string Name);