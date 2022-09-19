using System.ComponentModel.DataAnnotations;

namespace Resturants.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public List<Product> Product { get; set; }
    }
}
