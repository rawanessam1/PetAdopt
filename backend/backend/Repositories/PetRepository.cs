using backend.Data;
using backend.Models;
using backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        public PetRepository(AppDbContext context) : base(context)
        {
        }

        public new async Task<Pet> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id); // mawgouda aw laa
        }

        public new async Task<List<Pet>> GetAllAsync()
        {
            return await _dbSet.Include(p => p.Images).ToListAsync();
        }

        public async Task<List<Pet>> SearchAsync(string? type, string? location, string? breed, int? age)
        {
            var query = _dbSet.Include(p => p.Images).AsQueryable();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(p => p.Type.ToLower() == type.ToLower());

            if (!string.IsNullOrEmpty(location))
                query = query.Where(p => p.Location.ToLower() == location.ToLower());

            if (!string.IsNullOrEmpty(breed))
                query = query.Where(p => p.Breed.ToLower().Contains(breed.ToLower()));

            if (age.HasValue)
                query = query.Where(p => p.Age == age);

            // ONLY return pets that the Admin has approved!
            query = query.Where(p => p.Status == PetStatus.Available);

            return await query.ToListAsync();
        }

        public async Task<List<Pet>> GetPendingPetsAsync()
        {
            // CHANGED: Use the Enum
            return await _dbSet
                .Include(p => p.Images)
                .Where(p => p.Status == PetStatus.PendingApproval)
                .ToListAsync();
        }

        public async Task<List<Pet>> GetAvailablePetsSortedByAgeAsync()
        {
            // CHANGED: Use the Enum
            return await _dbSet
                .Include(p => p.Images)
                .Where(p => p.Status == PetStatus.Available)
                .OrderBy(p => p.Age)
                .ToListAsync();
        }
    }
}