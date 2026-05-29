using System;
using System.Threading.Tasks;
using CoffeShop.BL.Interfaces;
using CoffeShop.DL.Interfaces;
using CoffeShop.DL.Kafka;
using CoffeShop.Models.Responses;

namespace CoffeShop.BL.Services
{
    internal class SellCoffee : ISellCoffee
    {
        private readonly ICoffeeCrudService _coffeeCrudService;
        private readonly ICustomerRepository _customerRepository;
        private readonly GenericKafkaProducer<string, SellCoffeeResult> _producer;

        public SellCoffee(
            ICoffeeCrudService coffeeCrudService,
            ICustomerRepository customerRepository,
            GenericKafkaProducer<string, SellCoffeeResult> producer)
        {
            _coffeeCrudService = coffeeCrudService;
            _customerRepository = customerRepository;
            _producer = producer;
        }

        public async Task<SellCoffeeResult> Sell(Guid coffeeId, Guid customerId)
        {
            var coffee = await _coffeeCrudService.GetByIdAsync(coffeeId);
            var customer = await _customerRepository.GetById(customerId);

            if (coffee == null || customer == null)
                throw new ArgumentException("Coffee or Customer not found.");

            var price = coffee.BasePrice - customer.Discount;

            var result = new SellCoffeeResult
            {
                Price = price,
                Coffee = coffee,
                Customer = customer
            };

            await _producer.ProduceAsync(coffeeId.ToString(), result);

            return result;
        }
    }
}
