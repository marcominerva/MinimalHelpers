using MinimalHelpers.OpenApi;
using MinimalHelpers.Routing;
using MinimalSample.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddFormFile();

    // This is only needed with .NET 7.0.
    //options.AddMissingSchemas();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Maps all the endpoints within this Assembly.
app.MapEndpoints();

// Maps all the endpoints within the same Assembly of the specified type.
//app.MapEndpointsFromAssemblyContaining<SuppliersHandler>();

// Explicitly maps the endpoints of the specified type.
app.MapEndpoints<SuppliersHandler>();

// You can further decide what types to map using a predicate:
//app.MapEndpoints(type =>
//{
//    if (type.Name.StartsWith("Products"))
//    {
//        return false;
//    }

//    return true;
//});

app.MapGet("/api/schemas", (Guid id, DateTime dateTime, DateOnly date, TimeOnly time) => TypedResults.NoContent())
.WithOpenApi();

app.MapPost("/api/upload", (IFormFile file) =>
{
    return TypedResults.Ok(new { file.FileName, file.ContentType, file.Length });
})
.DisableAntiforgery()
.WithOpenApi(operation =>
{
    operation.Description = "If you use the 'WithOpenApi' extension method, you need to call the 'AddFormFile' method on the 'SwaggerGenOptions' instance to be sure that swagger.json file contains the right definition.";
    return operation;
});

app.MapPost("/api/multiupload", (IFormFileCollection files) =>
{
    return TypedResults.Ok(new { FileCount = files.Count });
})
.DisableAntiforgery()
.WithOpenApi(operation =>
{
    operation.Description = "If you use the 'WithOpenApi' extension method, you need to call the 'AddFormFile' method on the 'SwaggerGenOptions' instance to be sure that swagger.json file contains the right definition.";
    return operation;
});

app.Run();