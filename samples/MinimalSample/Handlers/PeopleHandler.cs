using System.ComponentModel.DataAnnotations;
using MinimalHelpers.Validation;

namespace MinimalSample.Handlers;

public class PeopleHandler : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/people", GetList);

        endpoints.MapGet("/api/people/{id:guid}", Get);

        endpoints.MapPost("/api/people", Insert)
            // MinimalHelpers.Validation package performs validation with Data Annotations.
            .WithValidation<Person>();

        endpoints.MapPut("/api/people/{id:guid}", Update)
            // MinimalHelpers.Validation package performs validation with Data Annotations.
            .WithValidation<Person>();

        endpoints.MapDelete("/api/people/{id:guid}", Delete);
    }

    private static IResult GetList() => TypedResults.NoContent();

    private static IResult Get(Guid id) => TypedResults.NoContent();

    private static IResult Insert(Person Person) => TypedResults.NoContent();

    private static IResult Update(Guid id, Person Person) => TypedResults.NoContent();

    private static IResult Delete(Guid id) => TypedResults.NoContent();
}

public class Person
{
    [Required]
    [MaxLength(20)]
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(20)]
    public string? LastName { get; set; }

    [MaxLength(50)]
    public string? City { get; set; }
}