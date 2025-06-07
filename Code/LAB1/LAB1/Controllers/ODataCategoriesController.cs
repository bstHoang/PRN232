using LAB1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace LAB1.Controllers
{
    public class ODataCategoriesController : ODataController
    {
        private readonly NewsWebsiteDbContext _context;

        public ODataCategoriesController(NewsWebsiteDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet]
        public IQueryable<Category> Get()
        {
            Console.WriteLine("ODataCategoriesController.Get called");
            return _context.Categories;
        }
    }
}