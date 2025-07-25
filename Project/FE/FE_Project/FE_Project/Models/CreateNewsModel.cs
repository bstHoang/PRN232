namespace FE_Project.Models
{
    public class CreateNewsModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
    }
}
