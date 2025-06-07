using AutoMapper;
using LAB1.DTOs;
using LAB1.Models;
using Microsoft.EntityFrameworkCore;

namespace LAB1.Services
{
    public class NewsService : INewsService
    {
        private readonly NewsWebsiteDbContext _context;
        private readonly IMapper _mapper;

        public NewsService(NewsWebsiteDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsDto>> GetAllNewsAsync()
        {
            var news = await _context.News.Include(n => n.Category).ToListAsync();
            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }

        public async Task<NewsDto> GetNewsByIdAsync(int id)
        {
            var news = await _context.News.Include(n => n.Category).FirstOrDefaultAsync(n => n.Id == id);
            if (news == null) throw new KeyNotFoundException("News not found");
            return _mapper.Map<NewsDto>(news);
        }

        public async Task<NewsDto> CreateNewsAsync(NewsDto newsDto)
        {
            var news = _mapper.Map<News>(newsDto);
            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return _mapper.Map<NewsDto>(news);
        }

        public async Task<NewsDto> UpdateNewsAsync(int id, NewsDto newsDto)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) throw new KeyNotFoundException("News not found");

            _mapper.Map(newsDto, news);
            await _context.SaveChangesAsync();
            return _mapper.Map<NewsDto>(news);
        }

        public async Task DeleteNewsAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) throw new KeyNotFoundException("News not found");

            _context.News.Remove(news);
            await _context.SaveChangesAsync();
        }
    }
}
