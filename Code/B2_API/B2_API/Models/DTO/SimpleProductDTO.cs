namespace B2_API.Models.DTO
{
    public class SimpleProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
