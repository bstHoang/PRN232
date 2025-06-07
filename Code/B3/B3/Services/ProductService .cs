using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B3.Models;
using B3.Models.IModels;

namespace B3.Services
{
    internal class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        public ProductService()
        {
            _context = new ProductDbContext(); // Dùng constructor mặc định có sẵn chuỗi kết nối
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
    }
}
