using CoffeShop.BL.Interfaces;
using CoffeShop.DL.Interfaces;
using CoffeShop.Models.Dto;

namespace CoffeShop.BL.Services
{
    internal class CoffeeCrudService : ICoffeeCrudService
    {
        private readonly ICoffeeRepository _coffeeRepository;

        public CoffeeCrudService(ICoffeeRepository coffeeRepository)
        {
            _coffeeRepository = coffeeRepository;
        }

        public void AddCoffee(Coffee coffee)
        {
            if (coffee == null) return;

            if (coffee.Id == Guid.Empty)
            {
                coffee.Id = Guid.NewGuid();
            }

            _coffeeRepository.AddCoffee(coffee);
        }

        public void DeleteCoffee(Guid id)
        {
            _coffeeRepository.DeleteCoffee(id);
        }

        public List<Coffee> GetAllCoffees()
        {
            return _coffeeRepository.GetAllCoffees();
        }

        public Coffee? GetById(Guid id)
        {
            return _coffeeRepository.GetById(id);
        }
    }
}
