using MessagePack;
using CoffeShop.Models.Dto;

namespace CoffeShop.Models.Responses
{
    [MessagePackObject]
    public class SellCoffeeResult
    {
        [Key(0)]
        public Coffee Coffee { get; set; }

        [Key(1)]
        public Customer Customer { get; set; }

        [Key(2)]
        public decimal Price { get; set; }
    }
}
