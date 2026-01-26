using CoffeShop.Models.Dto;

namespace CoffeShop.Tests.MockData
{
    internal class CoffeeMockedData
    {
        public static List<Coffee> Coffees = new List<Coffee>
        {
            new Coffee { Id = Guid.NewGuid(), Name = "Espresso", RoastYear = 2023, BasePrice = 3.50m },
            new Coffee { Id = Guid.NewGuid(), Name = "Cappuccino", RoastYear = 2023, BasePrice = 4.20m },
            new Coffee { Id = Guid.NewGuid(), Name = "Latte", RoastYear = 2023, BasePrice = 4.50m }
        };
    }
}
