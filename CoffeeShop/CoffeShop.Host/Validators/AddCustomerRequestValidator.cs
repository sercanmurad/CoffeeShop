using CoffeShop.Models.Requests;
using FluentValidation;

namespace CoffeShop.Host.Validators
{
    public class AddCustomerRequestValidator : AbstractValidator<AddCustomerRequest>
    {
        public AddCustomerRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name is required.")
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");
        }
    }
}
