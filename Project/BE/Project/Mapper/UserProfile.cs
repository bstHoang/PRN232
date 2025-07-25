using AutoMapper;
using Project.DTOs;
using Project.DTOs.Accounts;
using Project.Models;

namespace Project.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email.ToLower().Trim()))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower().Trim()))
                 .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper().Trim()))
                 .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Email.ToUpper().Trim()));
            CreateMap<ApplicationUser, AccountDto>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));
        }
    }
    
}
