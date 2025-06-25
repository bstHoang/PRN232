using _10API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace _10API.Controllers
{
    public class PetsControler : Controller
    {
        private readonly Api10TestContext _context;
        public PetsControler(Api10TestContext context)
        {
            _context = context;
        }
        //1 oke - get pet
        [EnableQuery]
        [HttpGet]
        [Route("api/pets/GetPets")]
        public IActionResult GetPets()
        {
            return Ok(_context.Pets);
        }

        [HttpGet]
        [Route("api/pets/GetPets/{id}")]
        public async Task<IActionResult> GetPetById(int id)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
                return NotFound();

            return Ok(pet);
        }
        //3 oke - create pet
        [HttpPost]
        [Route("api/pets/CreatePet")]
        public async Task<IActionResult> CreatePet([FromBody] Pet pet)
        {
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPets), new { id = pet.PetId }, pet);
        }
        //5 - delete pet by id
        [HttpDelete]
        [Route("api/pets/DeletePet/{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var pet = await _context.Pets
                .Include(p => p.Appointments)
                .Include(p => p.PetServices)
                .FirstOrDefaultAsync(p => p.PetId == id);

            if (pet == null) return NotFound();

            if (pet.Appointments.Any() || pet.PetServices.Any())
                return BadRequest("Không thể xoá vì thú cưng có liên quan đến dữ liệu khác.");

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Delete thanh cong" });
        }
        //8 oke -update pet by id
        [HttpPut]
        [Route("api/pets/UpdatePet/{id}")]
        public async Task<IActionResult> UpdatePet(int id, [FromBody] Pet pet)
        {
            if (id != pet.PetId) return BadRequest();

            _context.Entry(pet).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Update thanh cong" });
        }
        
    }
}
