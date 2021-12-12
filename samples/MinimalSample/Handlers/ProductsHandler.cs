using MinimalHelpers.Registration;

namespace MinimalSample.Handlers;

public class ProductsHandler : IEndpointRouteHandler
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", GetList);
        app.MapGet("/api/products/{id:guid}", Get);
        app.MapPost("/api/products", Insert);
        app.MapPut("/api/products/{id:guid}", Update);
        app.MapDelete("/api/products/{id:guid}", Delete);
    }

    private static IResult GetList() => Results.NoContent();

    private static IResult Get(Guid id) => Results.NoContent();

    private static IResult Insert(Product Person) => Results.NoContent();

    private static IResult Update(Product Person) => Results.NoContent();

    private static IResult Delete(Guid id) => Results.NoContent();
}

public record Product(string Name, string Description, double UnitPrice);

