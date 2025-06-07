using B2_API.Models.DTO;
using B2_API.Models;

namespace B2_API.Mapping
{
    public class MappingHelper
    {
        public static ProductDTO ToProductDTO(Product product)
        {
            if (product == null) return null!;

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Categories = product.Categories?.Select(c => ToCategoryDTO(c)).ToList() ?? new List<CategoryDTO>()
            };
        }

        public static CategoryDTO ToCategoryDTO(Category category)
        {
            if (category == null) return null!;

            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };
        }
        public static SimpleProductDTO ToSimpleProductDTO(Product product)
        {
            if (product == null) return null!;

            return new SimpleProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }

        public static CategoryWithProductsDTO ToCategoryWithProductsDTO(Category category)
        {
            if (category == null) return null!;

            return new CategoryWithProductsDTO
            {
                Id = category.Id,
                Name = category.Name,
                Products = category.Products?.Select(p => ToSimpleProductDTO(p)).ToList() ?? new List<SimpleProductDTO>()
            };
        }
    }
}