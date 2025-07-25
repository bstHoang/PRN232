using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs.Tags;
using Project.Interfaces;
using Project.Models;

namespace Project.Services
{
    public class TagService : ITagService
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;

        public TagService(ProjectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            Console.WriteLine("TagService.GetAllTagsAsync - Retrieving all tags");
            var tags = await _context.Tags.ToListAsync();
            var tagDtos = _mapper.Map<IEnumerable<TagDto>>(tags);
            Console.WriteLine($"TagService.GetAllTagsAsync - Found {tags.Count} tags");
            return tagDtos;
        }
    }
}