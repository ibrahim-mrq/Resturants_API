namespace Resturants.DTO.Requests
{
    public class CartRequest
    {
        public CartRequest()
        {
            Quantity = 1;
        }

        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
