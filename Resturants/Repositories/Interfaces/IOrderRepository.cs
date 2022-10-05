using Resturants.DTO.Requests;
using Resturants.Helper;

namespace Resturants.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        OperationType GetOrder(string Token, int UserId);
        OperationType GetOrderDetails(string Token, int UserId, int OrderId);
        OperationType AddNetOrder(string Token, int UserId, int cartId);
        OperationType ClearAllCart();
        OperationType DeleteCartById(int CartId);
        OperationType RemoveProductFromCart(string Token, int UserId, int CartId, int ProductId);

    }
}
