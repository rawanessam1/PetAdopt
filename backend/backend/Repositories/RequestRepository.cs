using backend.Models;
using backend.Data;
using backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class RequestRepository : GenericRepository<adoptionRequest>, IRequestRepository
    {
        public RequestRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<adoptionRequest>> GetByOwnerIdAsync(int ownerId)
        {
            return await _dbSet
                .Include(r => r.Pet)
                .Where(r => r.Pet.OwnerId == ownerId)
                .ToListAsync();
        }

        public async Task<List<adoptionRequest>> GetByPetIdAsync(int petId)
        {
            return await _dbSet
                .Where(r => r.PetId == petId)
                .ToListAsync();
        }

        public new async Task<adoptionRequest> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Pet)
                .FirstOrDefaultAsync(r => r.Id == id);
        }


    }
}
