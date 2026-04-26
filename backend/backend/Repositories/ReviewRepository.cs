using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Review>> GetByOwnerIdAsync(int ownerId)
        {
            return await _dbSet
                .Include(r => r.Adopter)
                .Where(r => r.OwnerId == ownerId)
                .ToListAsync();
        }
    }
}
