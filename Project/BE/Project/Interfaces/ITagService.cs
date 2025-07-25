using Microsoft.AspNetCore.Mvc;
using Project.DTOs.Tags;

namespace Project.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task<List<TagWithCountDto>> GetTopTags();
    }
}
