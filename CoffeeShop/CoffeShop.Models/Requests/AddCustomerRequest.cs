namespace CoffeShop.Models.Requests
{
    public class AddCustomerRequest
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Discount { get; set; }
    }
}
