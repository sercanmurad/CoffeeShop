using CoffeShop.DL.Interfaces;
using CoffeShop.DL.LocalDb;
using CoffeShop.Models.Dto;

namespace CoffeShop.DL.Repositories
{
    [Obsolete($"Please use: {nameof(CoffeeMongoRepository)}")]
    internal class CoffeeLocalRepository : ICoffeeRepository
    {
        public void AddCoffee(Coffee coffee)
        {
            StaticDb.Coffees.Add(coffee);
        }

        public void DeleteCoffee(Guid? id)
        {
            StaticDb.Coffees.RemoveAll(c => c.Id == id);
        }

        public List<Coffee> GetAllCoffees()
        {
            return StaticDb.Coffees;
        }

        public Coffee? GetById(Guid? id)
        {
            return StaticDb.Coffees.FirstOrDefault(c => c.Id == id);
        }
    }
}
