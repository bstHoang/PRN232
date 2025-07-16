namespace Project.DTOs.News
{
    public class NewsCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
    }
}
