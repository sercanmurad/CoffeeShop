using System;
using MessagePack;

namespace CoffeShop.Models.Dto
{
    [MessagePackObject]
    public class Customer
    {
        [Key(0)]
        public Guid Id { get; set; }

        [Key(1)]
        public string Name { get; set; } = string.Empty;

        [Key(2)]
        public string Email { get; set; } = string.Empty;

        [Key(3)]
        public int Discount { get; set; }
    }
}
