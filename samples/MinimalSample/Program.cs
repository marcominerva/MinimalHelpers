using MinimalHelpers.OpenApi;
using MinimalHelpers.Routing;
using MinimalSample.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddMissingSchemas();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Maps all the endpoints within this Assembly.
app.MapEndpoints();

// Maps all the endpoints within the same Assembly of the specified type.
app.MapEndpointsFromAssemblyContaining<SuppliersHandler>();

// You can further decide what types to map using a predicate:
//app.MapEndpoints(type =>
//{
//    if (type.Name.StartsWith("Products"))
//    {
//        return false;
//    }

//    return true;
//});

app.MapGet("/api/schemas", (Guid id, DateTime dateTime, DateOnly date, TimeOnly time) => TypedResults.NoContent());

app.Run();