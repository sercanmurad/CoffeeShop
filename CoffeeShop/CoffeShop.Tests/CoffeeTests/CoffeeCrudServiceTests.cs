using CoffeShop.BL.Services;
using CoffeShop.DL.Interfaces;
using CoffeShop.Models.Dto;
using CoffeShop.Tests.MockData;
using Moq;

namespace CoffeShop.Tests.CoffeeTests
{
    public class CoffeeCrudServiceTests
    {
        private readonly Mock<ICoffeeRepository> _coffeeRepositoryMock;

        public CoffeeCrudServiceTests()
        {
            _coffeeRepositoryMock = new Mock<ICoffeeRepository>();
        }

        [Fact]
        public void AddCoffeeTest_Ok()
        {
            var expectedCoffeeCount = CoffeeMockedData.Coffees.Count + 1;
            var id = Guid.NewGuid();
            var coffee = new Coffee()
            {
                Id = id,
                Name = "Americano",
                RoastYear = 2023,
                BasePrice = 3.00m
            };

            _coffeeRepositoryMock
                .Setup(repo => repo.AddCoffee(coffee))
                .Callback(() =>
                {
                    CoffeeMockedData.Coffees.Add(coffee);
                });

            var service = new CoffeeCrudService(_coffeeRepositoryMock.Object);

            service.AddCoffee(coffee);
            var resultCoffee = CoffeeMockedData.Coffees.FirstOrDefault(c => c.Id == id);

            Assert.NotNull(resultCoffee);
            Assert.Contains(coffee, CoffeeMockedData.Coffees);
            Assert.Equal(expectedCoffeeCount, CoffeeMockedData.Coffees.Count);
            Assert.Equal(id, resultCoffee.Id);
        }
    }
}
