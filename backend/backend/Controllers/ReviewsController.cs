using backend.DTOs.Reviews;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewsController(IReviewService service)
        {
            _service = service;
        }

        // Add Review
        [Authorize(Roles = "Adopter")]
        [HttpPost]
        public async Task<IActionResult> Add(CreateReviewDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.AddReviewAsync(dto, userId);

            if (!result) return BadRequest("Cannot add review");

            return Ok("Review added");
        }

        // Get Reviews
        [AllowAnonymous]
        [HttpGet("{ownerId}")]
        public async Task<IActionResult> Get(int ownerId)
        {
            var result = await _service.GetReviewsAsync(ownerId);

            return Ok(result);
        }
    }
}