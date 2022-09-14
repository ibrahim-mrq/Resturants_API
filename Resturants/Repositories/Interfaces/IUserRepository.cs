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
        OperationType Login(UserLogin userLogin);

        OperationType UserRegistration(UserRequest userRequest);
        OperationType VendorRegistration(VendorRequest vendorRequest);
        OperationType ClearAllUser();
        OperationType DeleteUser(int id);
        OperationType LoadProfile(int id, string token);
        OperationType UpdateVendor(int id, VendorRequest vendorRequest);
    }
}
