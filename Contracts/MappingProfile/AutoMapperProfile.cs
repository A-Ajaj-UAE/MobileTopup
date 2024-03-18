using AutoMapper;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Response;

namespace MobileTopup.Contracts.MappingProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<Account, AccountResponse>()
                .ForMember (dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                ;
            CreateMap<Beneficiary, BeneficiaryResponse>();

        }
    }
}
