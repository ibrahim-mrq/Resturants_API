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


        OperationType AddAddress(int UserId, string Token, List<AddressRequest> addressRequest);
        OperationType AddProduct(int UserId, string Token, List<ProductRequest> productRequest);
        OperationType AddPhoto(int UserId, string Token, List<PhotoRequest> photoRequest);


        OperationType RemoveAddress(int UserId, int AddressId, string Token);
        OperationType RemovePhoto(int UserId, int PhotoId, string Token);
        OperationType RemoveMenu(int UserId, int MenuId, string Token);


        OperationType ClearAllUser();
        OperationType DeleteUser(int Id);

    }
}
