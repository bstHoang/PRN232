namespace LAB1.DTOs
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        //public string CategoryName { get; set; }
    }
}
