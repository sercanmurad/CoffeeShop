using CoffeShop.Models.Dto;
using FluentValidation;

namespace CoffeShop.Host.Validators
{
    public class AddCustomerRequestValidator : AbstractValidator<Customer>
    {
        public AddCustomerRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id cannot be empty.");

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
                .WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
        }
    }
}
