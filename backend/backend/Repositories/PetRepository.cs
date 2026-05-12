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

        public new async Task<List<Pet>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.Images)
                .Include(p => p.user)
                .ToListAsync();                    // ← Removed status filter
        }

        public new async Task<Pet> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Images)
                .Include(p => p.user)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Pet>> SearchAsync(string? type, string? location, string? breed, int? age)
        {
            var query = _dbSet
                .Include(p => p.Images)
                .Include(p => p.user)
                .AsQueryable();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(p => p.Type.ToLower() == type.ToLower());

            if (!string.IsNullOrEmpty(location))
                query = query.Where(p => p.Location.ToLower() == location.ToLower());

            if (!string.IsNullOrEmpty(breed))
                query = query.Where(p => p.Breed.ToLower().Contains(breed.ToLower()));

            if (age.HasValue)
                query = query.Where(p => p.Age == age);

            // Removed hard-coded Available filter here
            return await query.ToListAsync();
        }

        public async Task<List<Pet>> GetPendingPetsAsync()
        {
            return await _dbSet
                .Include(p => p.Images)
                .Where(p => p.Status == PetStatus.PendingApproval)
                .ToListAsync();
        }

        public async Task<List<Pet>> GetAvailablePetsSortedByAgeAsync()
        {
            return await _dbSet
                .Include(p => p.Images)
                .Where(p => p.Status == PetStatus.Available)
                .OrderBy(p => p.Age)
                .ToListAsync();
        }

        public async Task<List<Pet>> GetByOwnerIdAsync(int ownerId)
        {
            return await _dbSet
                .Include(p => p.Images)
                .Where(p => p.OwnerId == ownerId)
                .ToListAsync();
        }
    }
}