using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.ProductName).NotEmpty();
        RuleFor(p => p.ProductName).MinimumLength(2);
        RuleFor(p => p.ProductName).Must(StartWithA).WithMessage("Ürün ismi A ile başlamalı.");
        
    }

    private bool StartWithA(string arg)
    {
        return arg.StartsWith("A");
    }
}