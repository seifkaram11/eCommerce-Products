using FluentValidation;
using Products.Core.DTOs;

namespace Products.Core.Validator;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(_=>_.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

        RuleFor(_=>_.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(_=>_.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(_=>_.Category)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(_=>_.Brand)
            .NotEmpty().WithMessage("BrandId is required.");
    }
}
