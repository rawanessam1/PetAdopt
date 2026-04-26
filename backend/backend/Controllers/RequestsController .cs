using backend.DTOs.Requests;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/requests")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _service;

        public RequestsController(IRequestService service)
        {
            _service = service;
        }

        //  Send Request (Adopter)
        [Authorize(Roles = "Adopter")]
        [HttpPost]
        public async Task<IActionResult> Send(CreateRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.SendRequestAsync(dto, userId);

            if (!result) return BadRequest("Cannot send request");

            return Ok("Request sent successfully");
        }

        // Get Requests (Owner)
        [Authorize(Roles = "Shelter,PetOwner")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.GetRequestsForOwnerAsync(ownerId);

            return Ok(result);
        }

        // Accept
        [Authorize(Roles = "Shelter,PetOwner")]
        [HttpPut("{id}/accept")]
        public async Task<IActionResult> Accept(int id)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.AcceptRequestAsync(id, ownerId);

            if (!result) return BadRequest("Cannot accept");

            return Ok("Accepted");
        }

        // Reject
        [Authorize(Roles = "Shelter,PetOwner")]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.RejectRequestAsync(id, ownerId);

            if (!result) return BadRequest("Cannot reject");

            return Ok("Rejected");
        }
    }
}