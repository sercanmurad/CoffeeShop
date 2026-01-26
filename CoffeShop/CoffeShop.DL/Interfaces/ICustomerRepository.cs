using CoffeShop.Models.Dto;

namespace CoffeShop.DL.Interfaces
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
        void DeleteCustomer(Guid id);
        List<Customer> GetAllCustomers();
        Customer? GetById(Guid id);
    }
}
