using System.ComponentModel.DataAnnotations;

namespace Resturants.Models
{
    public class Cart : BaseEntity
    {

        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public string ProductPhoto { get; set; }


    }
}
