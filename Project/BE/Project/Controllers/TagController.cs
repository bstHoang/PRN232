using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Project.Interfaces;

namespace Project.Controllers
{
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [EnableQuery]
        [AllowAnonymous]
        [Route("api/Tags/GetAllTags")]
        public async Task<IActionResult> GetAllTags()
        {
            Console.WriteLine("TagController.GetAllTags - Retrieving tags");
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        [HttpGet]
        [EnableQuery]
        [AllowAnonymous]
        [Route("api/Tags/GetTopTagsList")]
        public async Task<IActionResult> GetTopTagsList()
        {
            var tags = await _tagService.GetTopTags();
            return Ok(tags);
        }
    }
}