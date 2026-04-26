using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Favorite>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(f => f.Pet)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<Favorite> GetByUserAndPetAsync(int userId, int petId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PetId == petId);
        }
    }
}
