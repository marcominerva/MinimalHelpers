using MinimalHelpers.Routing;
using MinimalSample.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

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

app.Run();