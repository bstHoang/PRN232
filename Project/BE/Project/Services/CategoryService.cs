using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs.Categories;
using Project.Interfaces;
using Project.Models;

namespace Project.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ProjectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            Console.WriteLine("CategoryService.GetAllCategoriesAsync - Retrieving all categories");
            var categories = await _context.Categories.ToListAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            Console.WriteLine($"CategoryService.GetAllCategoriesAsync - Found {categories.Count} categories");
            return categoryDtos;
        }
        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) throw new Exception("Category not found");
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new Exception("Category not found.");

            if (string.IsNullOrWhiteSpace(categoryDto.Name))
                throw new Exception("Category name cannot be empty.");

            category.Name = categoryDto.Name;
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }


        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            // Kiểm tra xem có bài viết nào dùng category này không
            bool isUsed = await _context.News.AnyAsync(n => n.CategoryId == id);
            if (isUsed)
            {
                throw new Exception("Can't delete, have information in other tables.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}