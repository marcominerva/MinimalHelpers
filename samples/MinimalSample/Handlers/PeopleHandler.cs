namespace MinimalSample.Handlers;

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

    private static IResult GetList() => TypedResults.NoContent();

    private static IResult Get(Guid id) => TypedResults.NoContent();

    private static IResult Insert(Person Person) => TypedResults.NoContent();

    private static IResult Update(Guid id, Person Person) => TypedResults.NoContent();

    private static IResult Delete(Guid id) => TypedResults.NoContent();
}

public record class Person(string FirstName, string LastName);