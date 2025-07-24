using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Q1.Models;
using System.Threading.Tasks;

namespace Q1.Controllers
{
    public class APIController : Controller
    {
        private readonly EcommerceDbContext _context;
        public APIController(EcommerceDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [EnableQuery]
        [Route("/api/orders")]
        public async Task<IActionResult> GetDirectors()
        {
            var orders = await _context.Orders.Include(o => o.Customer).ToListAsync();

            if (orders == null)  return NoContent();

            var result = orders.Select( o => new OrderDTO { 
                orderId = o.OrderId,
                orderDate = o.OrderDate,
                customer = new CustomerDTO { 
                    customerId = o.Customer.CustomerId,
                    fullName = o.Customer.FullName
                }
            }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [EnableQuery]
        [Route("/api/products/tag/{tagId}")]
        public IActionResult GetProductByTagId(int tagid)
        {
            var products = _context.Products.Include(p => p.Tags).AsQueryable();

            if (products == null) return NoContent();

            var existTag = _context.Tags.FirstOrDefault(eT => eT.TagId == tagid);
            if (existTag == null) return NotFound();

            var result = new
            {

            };
            return Ok(products);
        }

        [HttpDelete]
        [Route("/api/customers/{customerId}")]
        public IActionResult DeleteCustomerById(int customerid) {

            var existCustomer = _context.Customers.FirstOrDefault(ecs => ecs.CustomerId == customerid);

            if (existCustomer == null) return NotFound();

            if (existCustomer.Orders.Any()) return BadRequest();

            _context.Customers.Remove(existCustomer);
            _context.SaveChanges();
            return Ok();
        }
    }
}
