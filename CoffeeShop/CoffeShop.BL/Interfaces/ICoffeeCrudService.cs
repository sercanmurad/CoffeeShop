using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeShop.Models.Dto;

namespace CoffeShop.BL.Interfaces
{
    public interface ICoffeeCrudService
    {
        Task AddCoffeeAsync(Coffee coffee);

        Task DeleteCoffeeAsync(Guid id);

        Task<List<Coffee>> GetAllCoffeesAsync();

        Task<Coffee?> GetByIdAsync(Guid id);
    }
}
