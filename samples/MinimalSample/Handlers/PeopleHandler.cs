using MinimalHelpers.Registration;

namespace MinimalSample.Handlers;

public class PeopleHandler : IRouteEndpointHandler
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/people", GetList);
        app.MapGet("/api/people/{id:guid}", Get);
        app.MapPost("/api/people", Insert);
        app.MapPut("/api/people/{id:guid}", Update);
        app.MapDelete("/api/people/{id:guid}", Delete);
    }

    private static IResult GetList() => Results.NoContent();

    private static IResult Get(Guid id) => Results.NoContent();

    private static IResult Insert(Person Person) => Results.NoContent();

    private static IResult Update(Person Person) => Results.NoContent();

    private static IResult Delete(Guid id) => Results.NoContent();
}

public record Person(string FirstName, string LastName);

