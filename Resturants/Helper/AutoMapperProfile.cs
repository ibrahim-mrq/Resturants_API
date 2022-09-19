using AutoMapper;
using Resturants.DTO.Requests;
using Resturants.DTO.Responses;
using Resturants.Models;

namespace Resturants.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<User, UserResponse>();
            CreateMap<User, VendorResponse>();

            CreateMap<UserUpdateRequest, User>();
            CreateMap<VendorUpdateRequest, User>();

            CreateMap<UserRequest, User>();
            CreateMap<VendorRequest, User>();


            CreateMap<AddressRequest, Address>();
            CreateMap<ProductRequest, Product>();
            CreateMap<PhotoRequest, Photo>();


            CreateMap<Cart, CartResponse>();
            CreateMap<CartRequest, Cart>();

        }
    }
}
