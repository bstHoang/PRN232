namespace Project.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}