using backend.DTOs.User;
using backend.Models;

namespace backend.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(RegisterDto dto);
        Task<string?> LoginAsync(LoginDto dto);
        Task<bool> ApproveAsync(int id);
        Task<bool> RejectAsync(int id);
        Task<List<User>> GetPendingUsersAsync();
    }
}