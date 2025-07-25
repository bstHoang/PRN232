using AutoMapper;
using Project.DTOs.News;
using Project.Models;

namespace Project.Mapper
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.NewsTags.Select(nt => nt.Tag.Name).ToList()));
            CreateMap<NewsCreateDto, News>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreateBy, opt => opt.Ignore()) // sẽ gán trong service
                .ForMember(dest => dest.Disable, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.NewsTags, opt => opt.Ignore()); // tùy bạn xử lý riêng trong service

            CreateMap<NewsUpdateDto, News>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreateBy, opt => opt.Ignore());
        }
    }
}
