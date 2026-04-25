using backend.Data;
using backend.DTOs.User;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public UserService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                IsApproved = false
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return "Registered successfully";
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return null;
            if (!user.IsApproved)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();    // for creating token
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);  // nafs ely fe program.cs

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);    // return token as string
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsApproved = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
           await _context.SaveChangesAsync();
            return true;
        }
    }
}
