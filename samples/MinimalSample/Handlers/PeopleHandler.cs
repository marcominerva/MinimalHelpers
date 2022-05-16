using MinimalHelpers.Routing;

namespace MinimalSample.Handlers;

public class PeopleHandler : IEndpointRouteHandler
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/people", GetList);
        endpoints.MapGet("/api/people/{id:guid}", Get);
        endpoints.MapPost("/api/people", Insert);
        endpoints.MapPut("/api/people/{id:guid}", Update);
        endpoints.MapDelete("/api/people/{id:guid}", Delete);
    }

    private static IResult GetList() => Results.NoContent();

    private static IResult Get(Guid id) => Results.NoContent();

    private static IResult Insert(Person Person) => Results.NoContent();

    private static IResult Update(Person Person) => Results.NoContent();

    private static IResult Delete(Guid id) => Results.NoContent();
}

public record class Person(string FirstName, string LastName);
