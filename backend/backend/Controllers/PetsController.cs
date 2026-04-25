using Microsoft.AspNetCore.Mvc;
using backend.DTOs.Pet;
using backend.Services;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/pets")]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _service;

        public PetsController(IPetService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pets = await _service.GetAllAsync();
            return Ok(pets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pet = await _service.GetByIdAsync(id);
            if (pet == null) return NotFound();
            return Ok(pet);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? type, string? location, string? breed, int? age)
        {
            var result = await _service.SearchAsync(type, location, breed, age);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePetDto dto)
        {
            int ownerId = 1;
            var pet = await _service.CreateAsync(dto, ownerId);
            return Ok(pet);
        }

        [HttpPost("{id}/adopt")]
        public async Task<IActionResult> Adopt(int id)
        {
            var success = await _service.RequestAdoptionAsync(id);
            if (!success) return BadRequest("Pet is not available for adoption.");
            return Ok("Adoption request submitted.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePetDto dto)
        {
            int ownerId = 1;

            var result = await _service.UpdateAsync(id, dto, ownerId);
            if (!result) return BadRequest("Not allowed");

            return Ok("Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int ownerId = 1;

            var result = await _service.DeleteAsync(id, ownerId);
            if (!result) return BadRequest("Not allowed");

            return Ok("Deleted");
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var success = await _service.ApprovePetAsync(id);

            if (!success)
                return BadRequest("Pet not found or it is not in a pending state.");

            return Ok("Pet approved successfully. It is now publicly available.");
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var success = await _service.RejectPetAsync(id);

            if (!success)
                return BadRequest("Pet not found or it is not in a pending state.");

            return Ok("Pet post has been rejected.");
        }
    }
}