using FluentValidation;


namespace Application.Features.Product.Commands
{
	public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
	{
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull().WithMessage("{PropertyName} is required!")
                .Length(2,10).WithMessage("{PropertyName} is between 2 & 10!");

            RuleFor(x => x.Description)
               .NotEmpty()
               .NotNull().WithMessage("{PropertyName} is required!")
               .MaximumLength(500);
		}
    }
}
