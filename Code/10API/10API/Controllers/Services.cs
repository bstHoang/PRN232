using _10API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace _10API.Controllers
{
    public class Services : Controller
    {
        private readonly Api10TestContext _context;
        public Services(Api10TestContext context)
        {
            _context = context;
        }
        //7
        [EnableQuery]
        [HttpGet("api/services")]
        public IActionResult GetServices()
        {
            return Ok(_context.Services.Include(s => s.PetServices));
        }
    }
}
