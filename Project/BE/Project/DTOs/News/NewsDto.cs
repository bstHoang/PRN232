namespace Project.DTOs.News
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public int CreateBy { get; set; }
        public bool Disable { get; set; }
    }
}
