using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeShop.BL.Interfaces;
using CoffeShop.DL.Interfaces;
using CoffeShop.Models.Dto;

namespace CoffeShop.BL.Services
{
    internal class CustomerCrudService : ICustomerCrudService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerCrudService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Add(Customer? customer)
        {
            if (customer == null) return;

            customer.Id = Guid.NewGuid();

            await _customerRepository.Add(customer);
        }

        public async Task<List<Customer>> GetAll()
        {
            return await _customerRepository.GetAll();
        }

        public async Task<Customer?> GetById(Guid id)
        {
            return await _customerRepository.GetById(id);
        }

        public async Task Delete(Guid id)
        {
            await _customerRepository.Delete(id);
        }
    }
}
