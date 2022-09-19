using Resturants.DTO.Requests;
using Resturants.Helper;

namespace Resturants.Repositories.Interfaces
{
    public interface ICartRepository
    {
        OperationType AddToCart(int UserId, string Token, CartRequest CartRequest);
        OperationType GetCart(int UserId, string Token);
    }
}
