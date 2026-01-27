using CoffeShop.Models.Responses;

namespace CoffeShop.BL.Interfaces
{
    internal interface ISellCoffee
    {
        SellCoffeeResult Sell(Guid coffeeId, Guid customerId);
    }
}
