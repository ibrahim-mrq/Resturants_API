using System.ComponentModel.DataAnnotations;

namespace Resturants.DTO.Responses
{
    public class CartResponse
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductPhoto { get; set; }

    }
}
