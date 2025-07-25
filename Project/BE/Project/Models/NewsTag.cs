namespace Project.Models
{
    public class NewsTag
    {
        public int Id_News { get; set; }
        public int Id_Tags { get; set; }
        public News News { get; set; }
        public Tag Tag { get; set; }
    }
}