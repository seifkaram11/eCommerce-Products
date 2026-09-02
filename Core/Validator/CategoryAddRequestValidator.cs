using FluentValidation;
using Products.Core.DTOs;

namespace Products.Core.Validator;

public class CategoryAddRequestValidator : AbstractValidator<CategoryAddRequest>
{
    public CategoryAddRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(255)
            .WithMessage("Name cannot exceed 255 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");
    }
}
