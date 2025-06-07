using AutoMapper;
using LAB1.DTOs;
using LAB1.Models;

namespace LAB1.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<News, NewsDto>();
                //.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<NewsDto, News>();
        }
    }
}
