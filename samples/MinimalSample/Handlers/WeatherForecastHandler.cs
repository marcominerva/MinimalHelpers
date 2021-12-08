namespace WebApplication1.Handlers;

public static class WeatherForecastHandler
{
    private static readonly string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public static void RegisterWeatherForecastEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/weatherforecast/{id}", GetForecast).WithName("GetWeatherForecast");
    }

    public static IResult GetForecast(int id, HttpContext httpContext)
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
           new WeatherForecast
           (
               DateTime.Now.AddDays(index),
               Random.Shared.Next(-20, 55),
               summaries[Random.Shared.Next(summaries.Length)]
           ))
            .ToArray();

        return Results.Ok(forecast);
    }
}
