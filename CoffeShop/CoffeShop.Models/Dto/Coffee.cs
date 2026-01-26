namespace CoffeShop.Models.Dto
{
    public class Coffee
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int RoastYear { get; set; }

        public decimal BasePrice { get; set; }
    }
}
