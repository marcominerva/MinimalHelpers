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
    //options.ValidationErrorTitleMessageFactory = (context, errors) => $"There was {errors.Values.Sum(v => v.Length)} error(s)";
});

builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Maps all the endpoints.
app.MapEndpoints();

app.Run();