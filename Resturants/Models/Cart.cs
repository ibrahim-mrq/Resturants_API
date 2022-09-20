using System.ComponentModel.DataAnnotations;

namespace Resturants.Models
{
    public class Cart
    {

        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? CustomerName { get; set; }
        public List<CartProduct> CartProducts { get; set; }
        public int ProdcutCount { get; set; }
    }
}
