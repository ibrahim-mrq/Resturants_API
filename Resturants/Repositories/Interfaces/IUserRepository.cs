using Resturants.DTO.Requests;
using Resturants.DTO.Responses;
using Resturants.Helper;
using Resturants.Models;

namespace Resturants.Repositories.Interfaces
{
    public interface IUserRepository
    {
        OperationType GetAllUsers();
        OperationType GetUsers();
        OperationType GetVendors();
        OperationType UserRegistration(UserRequest userRequest);
        OperationType VendorRegistration(VendorRequest vendorRequest);
        OperationType Login(UserLogin userLogin);
        OperationType ClearAllUser();
        OperationType DeleteUser(int id);
        OperationType LoadProfile(int id);
    }
}
