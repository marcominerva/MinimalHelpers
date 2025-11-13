using MinimalHelpers.FluentValidation;
using MinimalHelpers.OpenApi;

namespace MinimalSample.Endpoints;

public class PeopleEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/people", GetList);

        endpoints.MapGet("/api/people/{id:guid}", Get)
            //.ProducesProblem(StatusCodes.Status400BadRequest)
            //.ProducesProblem(StatusCodes.Status401Unauthorized)
            //.ProducesProblem(StatusCodes.Status403Forbidden)
            //.ProducesProblem(StatusCodes.Status404NotFound);
            .ProducesDefaultProblem(StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound);

        endpoints.MapPost("/api/people", Insert)
            // MinimalHelpers.FluentValidation package performs validation with Data Annotations.
            .WithValidation<Person>();

        endpoints.MapPut("/api/people/{id:guid}", Update)
            // MinimalHelpers.FluentValidation package performs validation with Data Annotations.
            .WithValidation<Person>();

        endpoints.MapDelete("/api/people/{id:guid}", Delete);
    }

    private static IResult GetList() => TypedResults.NoContent();

    private static IResult Get(Guid id) => TypedResults.NoContent();

    private static IResult Insert(Person Person) => TypedResults.NoContent();

    private static IResult Update(Guid id, Person Person) => TypedResults.NoContent();

    private static IResult Delete(Guid id) => TypedResults.NoContent();
}

public record class Person(string? FirstName, string? LastName, string? City);