using Project.DTOs.News;

namespace Project.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> GetAllNewsAsync();
        Task<IEnumerable<NewsDto>> GetNewsByJournalistAsync(int userId);
        Task<NewsDto> GetNewsByIdAsync(int id);
        Task<NewsDto> CreateNewsAsync(NewsCreateDto newsDto, int userId);
        Task UpdateNewsAsync(int id, NewsUpdateDto newsDto, int userId, string role);
        Task DeleteNewsAsync(int id, int userId, string role);
    }
}
