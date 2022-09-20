using Resturants.DTO.Requests;
using Resturants.Helper;

namespace Resturants.Repositories.Interfaces
{
    public interface ICartRepository
    {
        OperationType GetCart(string Token, int UserId);
        OperationType AddToCart(string Token, int UserId, int cartId, CartRequest CartRequest);
        /*    OperationType RemoveFromCart(string Token, int UserId, int CartId);
         OperationType UpdateProductInCart(string Token, int UserId, int CartId, int Quantity);
 */
    }
}
