using CoffeShop.Models.Dto;

namespace CoffeShop.BL.Interfaces
{
    public interface ICoffeeCrudService
    {
        void AddCoffee(Coffee coffee);

        void DeleteCoffee(Guid id);

        List<Coffee> GetAllCoffees();

        Coffee? GetById(Guid id);
    }
}
