using CoffeShop.BL.Interfaces;
using CoffeShop.DL.Interfaces;
using CoffeShop.Models.Dto;
using Moq;

namespace CoffeShop.Tests.CoffeeTests
{
    public class SellCoffeeTests
    {
        private Mock<ICoffeeCrudService> _coffeeCrudServiceMock;
        private Mock<ICustomerRepository> _customerRepositoryMock;

        [Fact]
        public void Sell_Return_Ok()
        {
            _coffeeCrudServiceMock = new Mock<ICoffeeCrudService>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            var expectedPrice = 2.50m;

            _coffeeCrudServiceMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Espresso",
                RoastYear = 2023,
                BasePrice = 3.50m
            });

            _customerRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new Customer
            {
                Id = Guid.NewGuid(),
                Email = "xxx@xxx.com",
                Discount = 1,
                Name = "John Doe"
            });

            var sellCoffeeService = new CoffeShop.BL.Services.SellCoffee(_coffeeCrudServiceMock.Object, _customerRepositoryMock.Object);

            var result = sellCoffeeService.Sell(Guid.NewGuid(), Guid.NewGuid());

            Assert.NotNull(result);
            Assert.Equal(expectedPrice, result.Price);
        }

        [Fact]
        public void Sell_When_Customer_Missing()
        {
            _coffeeCrudServiceMock = new Mock<ICoffeeCrudService>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();

            _coffeeCrudServiceMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new Coffee
            {
                Id = Guid.NewGuid(),
                Name = "Espresso",
                RoastYear = 2023,
                BasePrice = 3.50m
            });

            _customerRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns((Customer)null);

            var sellCoffeeService = new CoffeShop.BL.Services.SellCoffee(_coffeeCrudServiceMock.Object, _customerRepositoryMock.Object);

            Assert.Throws<ArgumentException>(() => sellCoffeeService.Sell(Guid.NewGuid(), Guid.NewGuid()));
        }
    }
}
