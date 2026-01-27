using CoffeShop.BL.Interfaces;
using CoffeShop.DL.Interfaces;
using CoffeShop.Models.Responses;

namespace CoffeShop.BL.Services
{
    internal class SellCoffee : ISellCoffee
    {
        private readonly ICoffeeCrudService _coffeeCrudService;
        private readonly ICustomerRepository _customerRepository;

        public SellCoffee(ICoffeeCrudService coffeeCrudService, ICustomerRepository customerRepository)
        {
            _coffeeCrudService = coffeeCrudService;
            _customerRepository = customerRepository;
        }

        public SellCoffeeResult Sell(Guid coffeeId, Guid customerId)
        {
            var coffee = _coffeeCrudService.GetById(coffeeId);
            var customer = _customerRepository.GetById(customerId);

            if (coffee == null || customer == null)
            {
                throw new ArgumentException($"Coffee with ID {coffeeId} not found.");
            }

            var price = coffee.BasePrice - customer.Discount;

            return new SellCoffeeResult
            {
                Price = price,
                Coffee = coffee,
                Customer = customer
            };
        }
    }
}
