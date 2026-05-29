using System;
using MessagePack;

namespace CoffeShop.DL.Kafka
{
    [MessagePackObject]
    public class SellCoffeeMessage
    {
        [Key(0)]
        public Guid CoffeeId { get; set; }

        [Key(1)]
        public Guid CustomerId { get; set; }

        [Key(2)]
        public decimal Price { get; set; }
    }
}
