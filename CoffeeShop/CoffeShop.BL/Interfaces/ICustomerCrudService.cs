using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeShop.Models.Dto;

namespace CoffeShop.BL.Interfaces
{
    public interface ICustomerCrudService
    {
        Task Add(Customer? customer);

        Task<List<Customer>> GetAll();

        Task<Customer?> GetById(Guid id);

        Task Delete(Guid id);
    }
}
