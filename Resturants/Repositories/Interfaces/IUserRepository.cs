using Resturants.DTO.Requests;
using Resturants.Helper;

namespace Resturants.Repositories.Interfaces
{
    public interface IUserRepository
    {
        OperationType GetAllUsers();
        OperationType GetUsers();
        OperationType GetVendors();


        OperationType Login(LoginRequest userLogin);
        OperationType LoadProfile(int Id, string token);
        

        OperationType UserRegistration(UserRequest userRequest);
        OperationType VendorRegistration(VendorRequest vendorRequest);


        OperationType UpdateVendor(int Id, string Token, VendorUpdateRequest vendorRequest);
        OperationType UpdateUser(int Id, string Token, UserUpdateRequest userUpdate);


        OperationType RemoveAddress(int UserId, int AddressId, string Token);


        OperationType ClearAllUser();
        OperationType DeleteUser(int Id);


    }
}
