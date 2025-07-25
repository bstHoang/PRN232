using Project.DTOs.News;
using System.Security.Claims;

namespace Project.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> GetAllNewsAsync();
        Task<IEnumerable<NewsDto>> GetNewsByJournalistAsync(int userId);
        Task<NewsDto> GetNewsByIdAsync(int id, ClaimsPrincipal user);
        Task<NewsDto> CreateNewsAsync(NewsCreateDto newsDto, int userId);
        Task UpdateNewsAsync(int id, NewsUpdateDto newsDto, int userId, string role);
        Task DeleteNewsAsync(int id, int userId, string role);
        Task<IEnumerable<NewsDto>> SearchNewsByTitleAsync(string title);
        Task<IEnumerable<NewsDto>> GetAllNewsForManagerAsync();
        Task<IEnumerable<NewsDto>> GetNewsByCategoryAsync(int categoryId);

        Task<List<NewsDto>> GetNewsByTagAsync(int tagId);
    }
}
