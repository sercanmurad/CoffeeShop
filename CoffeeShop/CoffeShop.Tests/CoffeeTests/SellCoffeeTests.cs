using System;
using System.Threading.Tasks;
using CoffeShop.BL.Interfaces;
using CoffeShop.BL.Services;
using CoffeShop.DL.Interfaces;
using CoffeShop.DL.Kafka;
using CoffeShop.Models.Dto;
using CoffeShop.Models.Responses;
using Moq;

namespace CoffeShop.Tests.CoffeeTests
{
    public class SellCoffeeTests
    {
        private Mock<ICoffeeCrudService> _coffeeCrudServiceMock = null!;
        private Mock<ICustomerRepository> _customerRepositoryMock = null!;

        [Fact]
        public async Task Sell_Return_Ok()
        {
            _coffeeCrudServiceMock = new Mock<ICoffeeCrudService>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            var expectedPrice = 2.50m;

            _coffeeCrudServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Espresso",
                RoastYear = 2023,
                BasePrice = 3.50m
            });

            _customerRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Customer
            {
                Id = Guid.NewGuid(),
                Email = "xxx@xxx.com",
                Discount = 1,
                Name = "John Doe"
            });

            var sellCoffeeService = new SellCoffee(
                _coffeeCrudServiceMock.Object,
                _customerRepositoryMock.Object,
                new TestKafkaProducer());

            var result = await sellCoffeeService.Sell(Guid.NewGuid(), Guid.NewGuid());

            Assert.NotNull(result);
            Assert.Equal(expectedPrice, result.Price);
        }

        [Fact]
        public async Task Sell_When_Customer_Missing()
        {
            _coffeeCrudServiceMock = new Mock<ICoffeeCrudService>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();

            _coffeeCrudServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Espresso",
                RoastYear = 2023,
                BasePrice = 3.50m
            });

            _customerRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((Customer?)null);

            var sellCoffeeService = new SellCoffee(
                _coffeeCrudServiceMock.Object,
                _customerRepositoryMock.Object,
                new TestKafkaProducer());

            await Assert.ThrowsAsync<ArgumentException>(() => sellCoffeeService.Sell(Guid.NewGuid(), Guid.NewGuid()));
        }

        private sealed class TestKafkaProducer : GenericKafkaProducer<string, SellCoffeeResult>
        {
            public TestKafkaProducer()
                : base(new KafkaSettings
                {
                    BootstrapServers = "localhost:9092",
                    SaslUsername = "test",
                    SaslPassword = "test",
                    Topic = "test-topic"
                })
            {
            }

            public override Task ProduceAsync(string key, SellCoffeeResult message) => Task.CompletedTask;
        }
    }
}
