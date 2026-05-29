using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeShop.DL.Interfaces;
using CoffeShop.DL.LocalDb;
using CoffeShop.Models.Dto;
using Microsoft.Extensions.Logging;

namespace CoffeShop.DL.Repositories
{
    [Obsolete($"Please use: {nameof(CustomerMongoRepository)}")]
    internal class CustomerLocalRepository : ICustomerRepository
    {
        private readonly ILogger<CustomerLocalRepository> _logger;

        public CustomerLocalRepository(ILogger<CustomerLocalRepository> logger)
        {
            _logger = logger;
        }

        public Task Add(Customer? customer)
        {
            if (customer != null)
            {
                StaticDb.Customers.Add(customer);
            }

            return Task.CompletedTask;
        }

        public Task<List<Customer>> GetAll()
        {
            try
            {
                return Task.FromResult(StaticDb.Customers);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAll)}:{e.Message}-{e.StackTrace}");
            }

            return Task.FromResult(new List<Customer>());
        }

        public Task<Customer?> GetById(Guid id)
        {
            if (id == Guid.Empty) return Task.FromResult<Customer?>(null);

            var customer = StaticDb.Customers.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(customer);
        }

        public async Task Delete(Guid id)
        {
            if (id == Guid.Empty) return;

            var customer = await GetById(id);

            if (customer != null)
            {
                StaticDb.Customers.Remove(customer);
            }
        }
    }
}
