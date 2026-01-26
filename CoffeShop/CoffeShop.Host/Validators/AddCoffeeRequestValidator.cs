using CoffeShop.Models.Requests;
using FluentValidation;

namespace CoffeShop.Host.Validators
{
    public class AddCoffeeRequestValidator : AbstractValidator<AddCoffeeRequest>
    {
        public AddCoffeeRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.")
                .MinimumLength(2).WithMessage("Name cannot be below 2 characters.")
                .WithMessage("Name is required.");

            RuleFor(x => x.RoastYear)
                .InclusiveBetween(1900, DateTime.Now.Year + 1)
                .WithMessage($"Roast year must be between 1900 and {DateTime.Now.Year + 1}.");
        }
    }
}
