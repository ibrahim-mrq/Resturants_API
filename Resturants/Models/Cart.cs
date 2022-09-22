using System.ComponentModel.DataAnnotations;

namespace Resturants.Models
{
    public class Cart
    {

        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? CustomerName { get; set; }
        public int TotleProduct { get; set; }
        public float TotlePrice { get; set; }
        public List<CartProduct> CartProducts { get; set; }
    }
}
