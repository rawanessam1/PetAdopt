using backend.DTOs.Requests;
using backend.Hubs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/requests")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _service;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IPetService _petService;

        public RequestsController(
            IRequestService service,
            IHubContext<NotificationHub> hub,
            IPetService petService)
        {
            _service = service;
            _hub = hub;
            _petService = petService;
        }

        // Send Request (Adopter)
        [Authorize(Roles = "Adopter")]
        [HttpPost]
        public async Task<IActionResult> Send(CreateRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.SendRequestAsync(dto, userId);
            if (!result) return BadRequest("Cannot send request");

            var pet = await _petService.GetByIdAsync(dto.PetId);
            if (pet != null)
            {
                // Notify the pet owner
                await _hub.Clients.Group($"owner_{pet.OwnerId}")
                    .SendAsync("NewRequest", new {
                        message = $"New adoption request for {pet.PetName}!",
                        petId = dto.PetId
                    });
            }

            return Ok("Request sent successfully");
        }

        // Get Requests (Owner)
        [Authorize(Roles = "Shelter,PetOwner")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.GetRequestsForOwnerAsync(ownerId);
            return Ok(result);
        }

        // Accept
        [Authorize(Roles = "Shelter,PetOwner")]
        [HttpPut("{id}/accept")]
        public async Task<IActionResult> Accept(int id)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.AcceptRequestAsync(id, ownerId);
            if (!result) return BadRequest("Cannot accept");

            // ✅ Get request to find adopter ID
            var request = await _service.GetRequestByIdAsync(id);
            if (request != null)
            {
                await _hub.Clients.Group($"user_{request.AdopterId}")
                    .SendAsync("RequestUpdated", new {
                        requestId = id,
                        status = "Accepted",
                        message = "Your adoption request was accepted! 🎉"
                    });
            }

            return Ok("Accepted");
        }

        // Reject
        [Authorize(Roles = "Shelter,PetOwner")]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.RejectRequestAsync(id, ownerId);
            if (!result) return BadRequest("Cannot reject");

            // ✅ Get request to find adopter ID
            var request = await _service.GetRequestByIdAsync(id);
            if (request != null)
            {
                await _hub.Clients.Group($"user_{request.AdopterId}")
                    .SendAsync("RequestUpdated", new {
                        requestId = id,
                        status = "Rejected",
                        message = "Your adoption request was rejected."
                    });
            }

            return Ok("Rejected");
        }

        // Get Requests for Adopter
        [Authorize(Roles = "Adopter")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMine()
        {
            var adopterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.GetRequestsForAdopterAsync(adopterId);
            return Ok(result);
        }

        // Get Adopter's history
        [Authorize(Roles = "Adopter")]
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var adopterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.GetAdopterHistoryAsync(adopterId);
            return Ok(result);
        }
    }
}