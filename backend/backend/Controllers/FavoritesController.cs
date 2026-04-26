using backend.DTOs.Favorites;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/favorites")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _service;

        public FavoritesController(IFavoriteService service)
        {
            _service = service;
        }

        // Add
        [Authorize(Roles = "Adopter")]
        [HttpPost]
        public async Task<IActionResult> Add(AddFavoriteDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.AddFavoriteAsync(dto, userId);

            if (!result) return BadRequest("Already exists or invalid");

            return Ok("Added to favorites");
        }

        // Remove
        [Authorize(Roles = "Adopter")]
        [HttpDelete("{petId}")]
        public async Task<IActionResult> Remove(int petId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.RemoveFavoriteAsync(petId, userId);

            if (!result) return NotFound();

            return Ok("Removed");
        }

        // List
        [Authorize(Roles = "Adopter")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.GetFavoritesAsync(userId);

            return Ok(result);
        }
    }
}