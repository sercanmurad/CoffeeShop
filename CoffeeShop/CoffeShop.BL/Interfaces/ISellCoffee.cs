using System;
using System.Threading.Tasks;
using CoffeShop.Models.Responses;

namespace CoffeShop.BL.Interfaces
{
    public interface ISellCoffee
    {
        Task<SellCoffeeResult> Sell(Guid coffeeId, Guid customerId);
    }
}
