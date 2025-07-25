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
        }
    }
}
