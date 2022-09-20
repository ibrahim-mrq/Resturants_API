using System.ComponentModel.DataAnnotations;

namespace Resturants.Models
{
    public class CartProduct
    {
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductPhoto { get; set; }


    }
}
