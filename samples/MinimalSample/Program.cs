using FluentValidation;
using MinimalHelpers.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.ConfigureValidation(options =>
{
    //options.ErrorResponseFormat = ErrorResponseFormat.List;

    // The default is "One or more validation errors occurred"
    //options.ValidationErrorMessageFactory = (context, errors) => $"There was {errors.Values.Sum(v => v.Length)} error(s)";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Maps all the endpoints.
app.MapEndpoints();

app.Run();