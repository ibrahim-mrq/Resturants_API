namespace Resturants.Models
{
    public class OrderDetails
    {

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public float Amount { get; set; }
        public string? ShippingAddress { get; set; }
        public string? DilivryDate { get; set; }
        public int Status { get; set; }
        public string? StatusTitle { get; set; }
        public List<Product>? Products { get; set; }

    }
}
