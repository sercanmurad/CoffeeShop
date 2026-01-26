using CoffeShop.DL.Interfaces;
using CoffeShop.DL.LocalDb;
using CoffeShop.Models.Dto;

namespace CoffeShop.DL.Repositories
{
    public class CustomerLocalRepository : ICustomerRepository
    {
        public void AddCustomer(Customer customer)
        {
            StaticDb.Customers.Add(customer);
        }

        public void DeleteCustomer(Guid id)
        {
            StaticDb.Customers.RemoveAll(c => c.Id == id);
        }

        public List<Customer> GetAllCustomers()
        {
            return StaticDb.Customers;
        }

        public Customer? GetById(Guid id)
        {
            return StaticDb.Customers.FirstOrDefault(c => c.Id == id);
        }
    }
}
