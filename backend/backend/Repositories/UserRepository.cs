using backend.Data;
using backend.Models;
using backend.Models.Enums;  
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetPendingUsersAsync()
        {
            return await _context.Users
                .Where(u => !u.IsApproved && u.Role != UserRole.Admin)
                .ToListAsync();
        }
    }
}