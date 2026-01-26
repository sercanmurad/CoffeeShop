using CoffeShop.Models.Dto;

namespace CoffeShop.BL.Interfaces
{
    public interface ICustomerCrudService
    {
        void AddCustomer(Customer customer);

        void DeleteCustomer(Guid id);

        List<Customer> GetAllCustomers();

        Customer? GetById(Guid id);
    }
}
