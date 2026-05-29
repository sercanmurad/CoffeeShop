using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeShop.DL.Interfaces;
using CoffeShop.DL.LocalDb;
using CoffeShop.Models.Dto;

namespace CoffeShop.DL.Repositories
{
    [Obsolete($"Please use: {nameof(CoffeeMongoRepository)}")]
    internal class CoffeeLocalRepository : ICoffeeRepository
    {
        public Task AddCoffeeAsync(Coffee coffee)
        {
            StaticDb.Coffees.Add(coffee);
            return Task.CompletedTask;
        }

        public Task DeleteCoffeeAsync(Guid? id)
        {
            StaticDb.Coffees.RemoveAll(c => c.Id == id);
            return Task.CompletedTask;
        }

        public Task<List<Coffee>> GetAllCoffeesAsync()
        {
            return Task.FromResult(StaticDb.Coffees);
        }

        public Task<Coffee?> GetByIdAsync(Guid? id)
        {
            var coffee = StaticDb.Coffees.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(coffee);
        }
    }
}
