using backend.DTOs.Pet;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pets = await _service.GetAllAsync();
            return Ok(pets);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pet = await _service.GetByIdAsync(id);
            if (pet == null) return NotFound();
            return Ok(pet);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> Search(string? type, string? location, string? breed, int? age)
        {
            var result = await _service.SearchAsync(type, location, breed, age);
            return Ok(result);
        }

        [Authorize(Roles = "Shelter,PetOwner")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePetDto dto)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var pet = await _service.CreateAsync(dto, ownerId);
            return Ok(pet);
        }

        [Authorize(Roles = "Adopter")]
        [HttpPost("{id}/adopt")]
        public async Task<IActionResult> Adopt(int id)
        {
            var success = await _service.RequestAdoptionAsync(id);
            if (!success) return BadRequest("Pet is not available for adoption.");
            return Ok("Adoption request submitted.");
        }

        [Authorize(Roles = "Shelter,PetOwner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePetDto dto)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.UpdateAsync(id, dto, ownerId);
            if (!result) return BadRequest("Not allowed");

            return Ok("Updated");
        }

        [Authorize(Roles = "Shelter,PetOwner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.DeleteAsync(id, ownerId);
            if (!result) return BadRequest("Not allowed");

            return Ok("Deleted");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var success = await _service.ApprovePetAsync(id);

            if (!success)
                return BadRequest("Pet not found or it is not in a pending state.");

            return Ok("Pet approved successfully. It is now publicly available.");
        }

        [Authorize(Roles = "Admin")]
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