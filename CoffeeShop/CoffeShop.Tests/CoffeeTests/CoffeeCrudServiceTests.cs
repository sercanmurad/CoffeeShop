using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task AddCoffeeTest_Ok()
        {
            var expectedCoffeeCount = CoffeeMockedData.Coffees.Count + 1;
            var id = Guid.NewGuid();
            var coffee = new Coffee()
            {
                Id = id,
                Name = "Americano",
                RoastYear = 2023
            };

            _coffeeRepositoryMock
               .Setup(repo => repo.AddCoffeeAsync(coffee))
               .Callback(() =>
               {
                   CoffeeMockedData.Coffees.Add(coffee);
               })
               .Returns(Task.CompletedTask);

            var service = new CoffeeCrudService(_coffeeRepositoryMock.Object);

            await service.AddCoffeeAsync(coffee);

            var resultCoffee = CoffeeMockedData.Coffees.FirstOrDefault(c => c.Id == id);

            Assert.NotNull(resultCoffee);
            Assert.Contains(coffee, CoffeeMockedData.Coffees);
            Assert.Equal(expectedCoffeeCount, CoffeeMockedData.Coffees.Count);
            Assert.Equal(id, resultCoffee.Id);
        }
    }
}
