using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DTOs.News;
using Project.Interfaces;
using Project.Models;
using System.Security.Claims;

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
                .Where(n => !n.Disable).OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }

        public async Task<IEnumerable<NewsDto>> GetNewsByJournalistAsync(int userId)
        {
            var news = await _context.News
                .Where(n => n.CreateBy == userId).OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }

        public async Task<NewsDto> GetNewsByIdAsync(int id, ClaimsPrincipal user)
        {
            var isManager = user.IsInRole("MANAGER");

            var query = _context.News
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag).OrderByDescending(n => n.CreatedAt)
                .AsQueryable();

            var news = await query.FirstOrDefaultAsync(n => n.Id == id);

            if (news == null)
                throw new Exception("News not found.");

            var newsDto = _mapper.Map<NewsDto>(news);
            Console.WriteLine($"NewsService.GetNewsByIdAsync - Id: {id}, Found Tags: {string.Join(", ", newsDto.Tags)}");
            return newsDto;
        }


        public async Task<NewsDto> CreateNewsAsync(NewsCreateDto newsDto, int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || !await _userManager.IsInRoleAsync(user, "JOURNALIST"))
                throw new Exception("Only journalists can create news.");

            var news = _mapper.Map<News>(newsDto);
            news.CreateBy = userId;

            if (newsDto.TagIds != null && newsDto.TagIds.Any())
            {
                foreach (var tagId in newsDto.TagIds)
                {
                    var tag = await _context.Tags.FindAsync(tagId);
                    news.NewsTags.Add(new NewsTag
                    {
                        Id_Tags = tagId,
                        Tag = tag,
                        Id_News = news.Id 
                    });
                }
            }

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return _mapper.Map<NewsDto>(news);
        }


        public async Task UpdateNewsAsync(int id, NewsUpdateDto newsDto, int userId, string role)
        {
            var news = await _context.News
                .Include(n => n.NewsTags)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (news == null)
            {
                Console.WriteLine($"NewsService.UpdateNewsAsync - News with Id {id} not found");
                throw new Exception("News not found.");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                Console.WriteLine($"NewsService.UpdateNewsAsync - User with Id {userId} not found");
                throw new Exception("User not found.");
            }

            if (role == "MANAGER")
            {
                news.Disable = newsDto.Disable; // Manager only updates Disable
                Console.WriteLine($"NewsService.UpdateNewsAsync - Manager updated Disable to {newsDto.Disable} for News Id {id}");
            }
            else if (role == "JOURNALIST" && news.CreateBy == userId)
            {
                _mapper.Map(newsDto, news); // Update Title, Description, Content, CategoryId, Disable

                // Validate CategoryId
                var category = await _context.Categories.FindAsync(newsDto.CategoryId);
                if (category == null)
                {
                    Console.WriteLine($"NewsService.UpdateNewsAsync - Category with Id {newsDto.CategoryId} not found");
                    throw new Exception($"Category with Id {newsDto.CategoryId} not found.");
                }

                // Update NewsTags
                var existingTagIds = news.NewsTags.Select(nt => nt.Id_Tags).ToList();
                var newTagIds = newsDto.TagIds ?? new List<int>();
                Console.WriteLine($"NewsService.UpdateNewsAsync - Existing Tags: {string.Join(", ", existingTagIds)}, New Tags: {string.Join(", ", newTagIds)}");

                // Validate new TagIds
                var validTagIds = await _context.Tags
                    .Where(t => newTagIds.Contains(t.Id))
                    .Select(t => t.Id)
                    .ToListAsync();
                if (validTagIds.Count != newTagIds.Count)
                {
                    var invalidTagIds = newTagIds.Except(validTagIds).ToList();
                    Console.WriteLine($"NewsService.UpdateNewsAsync - Invalid TagIds: {string.Join(", ", invalidTagIds)}");
                    throw new Exception($"Invalid TagIds: {string.Join(", ", invalidTagIds)}");
                }

                // Remove old NewsTags
                var tagsToRemove = news.NewsTags.Where(nt => !newTagIds.Contains(nt.Id_Tags)).ToList();
                _context.NewsTags.RemoveRange(tagsToRemove);

                // Add new NewsTags
                var tagsToAdd = newTagIds
                    .Where(tagId => !existingTagIds.Contains(tagId))
                    .Select(tagId => new NewsTag { Id_News = id, Id_Tags = tagId })
                    .ToList();
                _context.NewsTags.AddRange(tagsToAdd);
            }
            else
            {
                Console.WriteLine($"NewsService.UpdateNewsAsync - Unauthorized: UserId {userId}, Role {role}, CreateBy {news.CreateBy}");
                throw new Exception("Unauthorized to update this news.");
            }

            await _context.SaveChangesAsync();
            Console.WriteLine($"NewsService.UpdateNewsAsync - News Id {id} updated successfully");
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
                .Where(n => !n.Disable && n.Title.Contains(title)).OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            Console.WriteLine($"SearchNewsByTitleAsync - Found {news.Count} news with title containing: {title}");
            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }

        public async Task<IEnumerable<NewsDto>> GetAllNewsForManagerAsync()
        {
            var news = await _context.News
                .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
                .Include(n => n.CreatedBy).OrderByDescending(n => n.CreatedAt)
                .ToListAsync(); // không lọc Disable

            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }

        public async Task<IEnumerable<NewsDto>> GetNewsByCategoryAsync(int categoryId)
        {
            var news = await _context.News
                .Where(n => n.CategoryId == categoryId && !n.Disable)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<NewsDto>>(news);
        }
    }
}