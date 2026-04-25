using backend.DTOs.User;

namespace backend.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDto dto);

        Task<string?> LoginAsync(LoginDto dto);

        Task<bool> ApproveAsync(int id);

        Task<bool> RejectAsync(int id);
    }
}
