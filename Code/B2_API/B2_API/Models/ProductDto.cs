namespace B2_API.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Categories { get; set; }
    }
}
