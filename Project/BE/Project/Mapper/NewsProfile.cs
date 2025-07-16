using AutoMapper;
using Project.DTOs.News;
using Project.Models;

namespace Project.Mapper
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsDto>();
            CreateMap<NewsCreateDto, News>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreateBy, opt => opt.Ignore()) // Được đặt trong service
                .ForMember(dest => dest.Disable, opt => opt.MapFrom(src => false));
            CreateMap<NewsUpdateDto, News>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreateBy, opt => opt.Ignore());
        }
    }
}
