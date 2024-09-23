using FluentValidation;
using MinimalSample.Handlers;

namespace MinimalSample.Validations;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MaximumLength(50);
        RuleFor(p => p.Description).MaximumLength(500);
        RuleFor(p => p.UnitPrice).GreaterThan(0);
    }
}
