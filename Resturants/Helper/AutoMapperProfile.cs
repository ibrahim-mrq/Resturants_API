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

            CreateMap<UserRequest, User>().ForMember(opt => opt.IsDelete, opt => opt.MapFrom(src => false));
            CreateMap<VendorRequest, User>().ForMember(opt => opt.IsDelete, opt => opt.MapFrom(src => false));

        }
    }
}
