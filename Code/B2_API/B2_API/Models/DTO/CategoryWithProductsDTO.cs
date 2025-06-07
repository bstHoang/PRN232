namespace B2_API.Models.DTO
{
    public class CategoryWithProductsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<SimpleProductDTO> Products { get; set; } = new List<SimpleProductDTO>();
    }
}
