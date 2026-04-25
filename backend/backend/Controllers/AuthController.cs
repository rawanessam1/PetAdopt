using backend.DTOs.User;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;

        public AuthController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto dto)
        {
            var result = await _service.RegisterAsync(dto);
            return Ok(result);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto dto)
        {
            var token = await _service.LoginAsync(dto);
            if (token == null)
                return BadRequest("Invalid email or password or not approved");

            return Ok(new {token});
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _service.ApproveAsync(id);

            if (!result)
                return NotFound("User not found");

            return Ok("User approved");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _service.RejectAsync(id);

            if (!result)
                return NotFound("User not found");

            return Ok("User rejected");
        }
    }
}
