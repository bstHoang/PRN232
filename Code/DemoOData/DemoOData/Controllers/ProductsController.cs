using DemoOData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace DemoOData.Controllers
{
    [ApiController]
    [Route("gadget")]
    public class ProductsController : Controller
    {
        private readonly DemoOdataContext _context;
        public ProductsController(DemoOdataContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet("Get")]
        public IActionResult Get() { 
            return Ok(_context.Products.AsQueryable());
        }
    }
}
