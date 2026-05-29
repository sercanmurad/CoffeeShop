using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task AddCoffeeAsync(Coffee coffee)
        {
            if (coffee == null) return;

            if (coffee.Id == Guid.Empty)
            {
                coffee.Id = Guid.NewGuid();
            }

            await _coffeeRepository.AddCoffeeAsync(coffee);
        }

        public async Task DeleteCoffeeAsync(Guid id)
        {
            await _coffeeRepository.DeleteCoffeeAsync(id);
        }

        public async Task<List<Coffee>> GetAllCoffeesAsync()
        {
            return await _coffeeRepository.GetAllCoffeesAsync();
        }

        public async Task<Coffee?> GetByIdAsync(Guid id)
        {
            return await _coffeeRepository.GetByIdAsync(id);
        }
    }
}
