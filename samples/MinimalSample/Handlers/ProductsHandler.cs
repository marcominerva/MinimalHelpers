using MinimalHelpers.Routing;

namespace MinimalSample.Handlers;

public class ProductsHandler : IEndpointRouteHandler
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/products", GetList);
        endpoints.MapGet("/api/products/{id:guid}", Get);
        endpoints.MapPost("/api/products", Insert);
        endpoints.MapPut("/api/products/{id:guid}", Update);
        endpoints.MapDelete("/api/products/{id:guid}", Delete);
    }

    private static IResult GetList() => TypedResults.NoContent();

    private static IResult Get(Guid id) => TypedResults.NoContent();

    private static IResult Insert(Product Person) => TypedResults.NoContent();

    private static IResult Update(Product Person) => TypedResults.NoContent();

    private static IResult Delete(Guid id) => TypedResults.NoContent();
}

public record class Product(string Name, string Description, double UnitPrice);
