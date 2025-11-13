using FluentValidation;
using MinimalSample.Endpoints;

namespace MinimalSample.Validations;

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().MaximumLength(20);
        RuleFor(p => p.LastName).NotEmpty().MaximumLength(20);
        RuleFor(p => p.City).MaximumLength(50);
    }
}