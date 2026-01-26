using CoffeShop.Models.Dto;

namespace CoffeShop.Models.Responses
{
    public class SellCoffeeResult
    {
        public Coffee Coffee { get; set; }

        public Customer Customer { get; set; }

        public decimal Price { get; set; }
    }
}
