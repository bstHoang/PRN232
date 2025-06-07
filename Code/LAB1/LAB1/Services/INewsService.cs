using LAB1.DTOs;

namespace LAB1.Services
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> GetAllNewsAsync();
        Task<NewsDto> GetNewsByIdAsync(int id);
        Task<NewsDto> CreateNewsAsync(NewsDto newsDto);
        Task<NewsDto> UpdateNewsAsync(int id, NewsDto newsDto);
        Task DeleteNewsAsync(int id);
    }
}
