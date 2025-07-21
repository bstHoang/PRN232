using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DTOs.News;
using Project.Interfaces;
using Project.Models;

namespace Project.Services
{
    public class NewsService : INewsService
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public NewsService(ProjectDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<NewsDto>> GetAllNewsAsync()
        {
            var news = await _context.News
                .Where(n => !n.Disable)
                .ToListAsync();
            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }

        public async Task<IEnumerable<NewsDto>> GetNewsByJournalistAsync(int userId)
        {
            var news = await _context.News
                .Where(n => n.CreateBy == userId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }

        public async Task<NewsDto> GetNewsByIdAsync(int id)
        {
            var news = await _context.News
                .FirstOrDefaultAsync(n => n.Id == id && !n.Disable);
            if (news == null)
                throw new Exception("News not found.");
            return _mapper.Map<NewsDto>(news);
        }

        public async Task<NewsDto> CreateNewsAsync(NewsCreateDto newsDto, int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || !await _userManager.IsInRoleAsync(user, "JOURNALIST"))
                throw new Exception("Only journalists can create news.");

            var news = _mapper.Map<News>(newsDto);
            news.CreateBy = userId;
            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return _mapper.Map<NewsDto>(news);
        }

        public async Task UpdateNewsAsync(int id, NewsUpdateDto newsDto, int userId, string role)
        {
            var news = await _context.News.FirstOrDefaultAsync(n => n.Id == id);
            if (news == null)
                throw new Exception("News not found.");

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception("User not found.");

            if (role == "MANAGER")
            {
                news.Disable = newsDto.Disable; // Manager chỉ cần cập nhật Disable
            }
            else if (role == "JOURNALIST" && news.CreateBy == userId)
            {
                _mapper.Map(newsDto, news); // Journalist cập nhật toàn bộ thông tin
            }
            else
            {
                throw new Exception("Unauthorized to update this news.");
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteNewsAsync(int id, int userId, string role)
        {
            var news = await _context.News.FirstOrDefaultAsync(n => n.Id == id);
            if (news == null)
                throw new Exception("News not found.");

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception("User not found.");

            if (role == "MANAGER" || (role == "JOURNALIST" && news.CreateBy == userId))
            {
                _context.News.Remove(news);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Unauthorized to delete this news.");
            }
        }

        public async Task<IEnumerable<NewsDto>> SearchNewsByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("SearchNewsByTitleAsync - Title is empty, returning all news.");
                return await GetAllNewsAsync();
            }

            var news = await _context.News
                .Where(n => !n.Disable && n.Title.Contains(title))
                .ToListAsync();
            Console.WriteLine($"SearchNewsByTitleAsync - Found {news.Count} news with title containing: {title}");
            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }
    }
}