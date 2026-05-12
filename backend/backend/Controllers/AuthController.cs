using backend.DTOs.User;
using backend.Hubs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IHubContext<NotificationHub> _hub;

        public AuthController(IUserService service, IHubContext<NotificationHub> hub)
        {
            _service = service;
            _hub = hub;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _service.RegisterAsync(dto);

            // ✅ Notify admin if Shelter or PetOwner registers
            if (dto.Role == backend.Models.Enums.UserRole.Shelter || 
                dto.Role == backend.Models.Enums.UserRole.PetOwner)
            {
                await _hub.Clients.Group("admin_group")
                    .SendAsync("NewUserPending", new {
                        message = $"New {dto.Role} '{dto.Name}' is waiting for approval!"
                    });
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _service.LoginAsync(dto);
            if (token == null)
                return BadRequest("Invalid email or password or not approved");
            return Ok(new { token });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _service.ApproveAsync(id);
            if (!result) return NotFound("User not found");
            return Ok("User approved");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _service.RejectAsync(id);
            if (!result) return NotFound("User not found");
            return Ok("User rejected");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _service.GetPendingUsersAsync();
            return Ok(result);
        }
    }
}