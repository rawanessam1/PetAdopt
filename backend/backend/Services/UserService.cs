using backend.DTOs.User;
using backend.Models;
using backend.Models.Enums;
using backend.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public UserService(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                IsApproved = dto.Role == UserRole.Adopter
            };
            await _repo.AddAsync(user);
            return user;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null) return null;
            if (string.IsNullOrEmpty(user.Password)) return null;
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) return null;
            if (!user.IsApproved) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return false;
            user.IsApproved = true;
            await _repo.UpdateAsync(user);
            return true;
        }

        public async Task<bool> RejectAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return false;
            await _repo.DeleteAsync(user);
            return true;
        }

        public async Task<List<User>> GetPendingUsersAsync()
        {
            return await _repo.GetPendingUsersAsync();
        }
    }
}