using CompanyManage.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManage.APIControllers
{
    [Route("api/PositionsAPI")]
    [ApiController]
    public class PositionsAPI : Controller
    {
        private readonly CompanyDbContext _context;
        public PositionsAPI(CompanyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetPositions()
        {
            var positions = _context.Positions
                .Select(p => new { p.Id, p.Name })
                .ToList();
            return Ok(positions);
        }
    }
}
