namespace CoffeShop.Models.Requests
{
    public class AddCoffeeRequest
    {
        public string Name { get; set; } = string.Empty;
        public int RoastYear { get; set; }
    }
}
