using Microsoft.AspNetCore.Mvc;
using backend.DTOs.Pet;
using backend.Services;

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
        public IActionResult GetAll() //IActionResult to return status code with the retruning result
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var pet = _service.GetById(id);
            if (pet == null) return NotFound();
            return Ok(pet);
        }

        [HttpGet("search")]
        public IActionResult Search(string? type, string? location, string?breed, int? age)
        {
            var result = _service.Search(type, location, breed, age);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(CreatePetDto dto)
        {
            int ownerId = 1;
            var pet = _service.Create(dto, ownerId);
            return Ok(pet);
        }

        [HttpPost("{id}/adopt")]
        public IActionResult Adopt(int id)
        {
            var success = _service.RequestAdoption(id);
            if (!success) return BadRequest("Pet is not available for adoption.");
            return Ok("Adoption request submitted.");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdatePetDto dto)
        {
            int ownerId = 1;

            var result = _service.Update(id, dto, ownerId);
            if (!result) return BadRequest("Not allowed");

            return Ok("Updated");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int ownerId = 1;

            var result = _service.Delete(id, ownerId);
            if (!result) return BadRequest("Not allowed");

            return Ok("Deleted");
        }

    }
}