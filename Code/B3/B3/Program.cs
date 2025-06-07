using System;
using B3.Models.IModels;
using B3.Services;

class Program
{
    static void Main(string[] args)
    {
        IProductService productService = new ProductService();
        var products = productService.GetAllProducts();

        Console.WriteLine("== Danh sách sản phẩm ==");
        foreach (var p in products)
        {
            Console.WriteLine($"ID: {p.Id}");
            Console.WriteLine($"Tên: {p.Name}");
            Console.WriteLine($"Mô tả: {p.Description}");
            Console.WriteLine($"Giá: {p.Price} USD");
            Console.WriteLine($"Số lượng: {p.Quantity}");
            Console.WriteLine("------------------------------");
        }

        Console.ReadLine();
    }
}
