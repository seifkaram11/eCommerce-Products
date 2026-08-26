using FluentValidation;
using Products.Core.DTOs;

namespace Products.Core.Validator;

public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
{
    public ProductAddRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(255)
            .WithMessage("Name cannot exceed 255 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("CategoryId is required.");

        RuleFor(x => x.Brand)
            .NotEmpty()
            .WithMessage("BrandId is required.");
    }
}
