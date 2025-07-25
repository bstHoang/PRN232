using AutoMapper;
using Project.DTOs.Categories;
using Project.DTOs.News;
using Project.DTOs.Tags;
using Project.Models;

namespace Project.Mapper
{
    public class TagsProfile : Profile
    {
        public TagsProfile()
        {
            CreateMap<Tag, TagDto>();
            CreateMap<Tag, TagWithCountDto>()
            .ForMember(dest => dest.TagId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.NewsTags.Count));
        }
    }
}
