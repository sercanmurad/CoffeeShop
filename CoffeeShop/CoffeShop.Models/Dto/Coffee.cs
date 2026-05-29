using System;
using MessagePack;

namespace CoffeShop.Models.Dto
{
    [MessagePackObject]
    public class Coffee
    {
        [Key(0)]
        public Guid Id { get; set; }

        [Key(1)]
        public string Name { get; set; } = string.Empty;

        [Key(2)]
        public int RoastYear { get; set; }

        [Key(3)]
        public decimal BasePrice { get; set; }
    }
}
