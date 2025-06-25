using _10API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace _10API.Controllers
{
    public class PetServicesController : Controller
    {
        private readonly Api10TestContext _context;
        public PetServicesController(Api10TestContext context)
        {
            _context = context;
        }
        //2 oke - get services by petid
        [HttpGet]
        [Route("api/PetServices/GetServicesByPet/{petId}")]
        public IActionResult GetServicesByPet(int petId)
        {
            var services = _context.PetServices
                .Where(ps => ps.PetId == petId)
                .Include(ps => ps.Service)
                .Select(ps => new {
                    ps.Service.Name,
                    ps.ServiceDate,
                    ps.Note
                });

            return Ok(services);
        }
        //4 oke - Create PetService
        [HttpPost]
        [Route("api/PetServices/CreatePetService")]
        public async Task<IActionResult> CreatePetService([FromBody] PetService ps)
        {
            var exists = await _context.PetServices
                .AnyAsync(x => x.PetId == ps.PetId && x.ServiceId == ps.ServiceId && x.ServiceDate == ps.ServiceDate);

            if (exists) return BadRequest("Dịch vụ này đã được đăng ký cho thú cưng vào ngày đó.");

            _context.PetServices.Add(ps);
            await _context.SaveChangesAsync();
            return Ok(ps);
        }
        //6  oke - delete pet service
        [HttpDelete]
        [Route("api/PetServices/DeletePetService")]
        public async Task<IActionResult> DeletePetService([FromBody] PetService ps)
        {
            var item = await _context.PetServices
                .FirstOrDefaultAsync(x => x.PetId == ps.PetId && x.ServiceId == ps.ServiceId && x.ServiceDate == ps.ServiceDate);

            if (item == null) return NotFound();

            _context.PetServices.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Delete thanh cong" });
        }
        //11 oke - get all pet services
        [HttpGet]
        [EnableQuery]
        [Route("api/PetServices/GetAllPetServices")]
        public IActionResult GetAllPetServices()
        {
            var petServices = _context.PetServices
                .Include(ps => ps.Pet)
                .Include(ps => ps.Service)
                .Select(ps => new
                {
                    ps.PetId,
                    PetName = ps.Pet.Name,
                    ps.ServiceId,
                    ServiceName = ps.Service.Name,
                    ps.ServiceDate,
                    ps.Note
                })
                .ToList();

            return Ok(petServices);
        }
        //12 oke - update pet service
        [HttpPut]
        [Route("api/PetServices/UpdatePetService")]
        public async Task<IActionResult> UpdatePetService([FromBody] PetService updated)
        {
            var petService = await _context.PetServices
                .FirstOrDefaultAsync(x =>
                    x.PetId == updated.PetId &&
                    x.ServiceId == updated.ServiceId &&
                    x.ServiceDate == updated.ServiceDate);

            if (petService == null)
                return NotFound("Không tìm thấy bản ghi để cập nhật.");

            // Ví dụ cập nhật Note và đổi ngày dịch vụ
            petService.Note = updated.Note;
            petService.ServiceDate = updated.ServiceDate; // Nếu bạn cho phép đổi ngày

            await _context.SaveChangesAsync();

            return Ok(petService);
        }

        [HttpGet]
        [Route("api/PetServices/UpdatePetService/{petname}/{year}")]
        public IActionResult GetServicesBypetnameandyear(string petname, int year)
        {
            var services = _context.PetServices
                                .Include(ps => ps.Pet)
                                .Include(ps => ps.Service)
                                .AsEnumerable()
         .Where(ps => ps.Pet.Name.Contains(petname) &&
                      ps.ServiceDate.Year == year)
         .Select(ps => new {
             ps.Service.Name,
             ps.ServiceDate,
             ps.Note
         })
         .ToList();

            return Ok(services);
        }

    }
}
