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
    }
}