using FluentValidation;
using Products.Core.DTOs;

namespace Products.Core.Validator;

public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
{
    public CategoryUpdateRequestValidator()
    {
        RuleFor(_=>_.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

        RuleFor(_=>_.Description)
            .NotEmpty().WithMessage("Description is required.");
    }
}
