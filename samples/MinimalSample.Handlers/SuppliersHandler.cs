using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MinimalHelpers.Registration;

namespace MinimalSample.Handlers;

public class SuppliersHandler : IEndpointRouteHandler
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/suppliers", GetList);
        app.MapGet("/api/suppliers/{id:guid}", Get);
        app.MapPost("/api/suppliers", Insert);
        app.MapPut("/api/suppliers/{id:guid}", Update);
        app.MapDelete("/api/suppliers/{id:guid}", Delete);
    }

    private static IResult GetList() => Results.NoContent();

    private static IResult Get(Guid id) => Results.NoContent();

    private static IResult Insert(Supplier Person) => Results.NoContent();

    private static IResult Update(Supplier Person) => Results.NoContent();

    private static IResult Delete(Guid id) => Results.NoContent();
}

public record Supplier(string Name);