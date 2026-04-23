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
        private readonly UserService _service;

        public AuthController(UserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var result = _service.Register(dto);
            return Ok(result);

        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var token = _service.Login(dto);
            if (token == null)
                return BadRequest("Invalid email or password or not approved");

            return Ok(new {token});
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approve/{id}")]
        public IActionResult Approve(int id)
        {
            var result = _service.Approve(id);

            if (!result)
                return NotFound();

            return Ok("User approved");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("reject/{id}")]
        public IActionResult Reject(int id)
        {
            var result = _service.Reject(id);

            if (!result)
                return NotFound();

            return Ok("User rejected");
        }
    }
}
