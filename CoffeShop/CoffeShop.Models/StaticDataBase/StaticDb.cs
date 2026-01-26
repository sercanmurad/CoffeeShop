using CoffeShop.Models.Dto;

namespace CoffeShop.Models.StaticDataBase
{
    public static class StaticDb
    {
        public static List<Coffee> Coffees { get; set; } = new List<Coffee>()
        {
            new Coffee() { Id = Guid.NewGuid(), Name = "Espresso", RoastYear = 2023, BasePrice = 3.50m },
            new Coffee() { Id = Guid.NewGuid(), Name = "Cappuccino", RoastYear = 2023, BasePrice = 4.20m },
            new Coffee() { Id = Guid.NewGuid(), Name = "Latte", RoastYear = 2023, BasePrice = 4.50m },
        };
    }
}
