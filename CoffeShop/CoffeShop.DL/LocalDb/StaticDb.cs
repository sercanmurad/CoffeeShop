using CoffeShop.Models.Dto;

namespace CoffeShop.DL.LocalDb
{
    internal static class StaticDb
    {
        public static List<Coffee> Coffees = new List<Coffee>
        {
            new Coffee { Id = Guid.NewGuid(), Name = "Espresso", RoastYear = 2023, BasePrice = 3.50m },
            new Coffee { Id = Guid.NewGuid(), Name = "Cappuccino", RoastYear = 2023, BasePrice = 4.20m },
            new Coffee { Id = Guid.NewGuid(), Name = "Latte", RoastYear = 2021, BasePrice = 4.50m }
        };

        public static List<Customer> Customers =
            new List<Customer>()
            {
                new Customer()
                {
                    Id = Guid.NewGuid(),
                    Name = "Ivan Ivanov",
                    Email = "iv@xxx.com"
                },
                new Customer()
                {
                    Id = Guid.NewGuid(),
                    Name = "Sercan Murad",
                    Email = "sm@xxx.com"
                }
            };
    }
}
