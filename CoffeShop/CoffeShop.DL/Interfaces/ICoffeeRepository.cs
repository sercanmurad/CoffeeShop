using CoffeShop.Models.Dto;

namespace CoffeShop.DL.Interfaces
{
    public interface ICoffeeRepository
    {
        void AddCoffee(Coffee coffee);

        void DeleteCoffee(Guid? id);

        List<Coffee> GetAllCoffees();

        Coffee? GetById(Guid? id);
    }
}
